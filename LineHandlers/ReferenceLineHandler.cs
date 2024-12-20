﻿using CopyChanges.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace CopyChanges.LineHandlers
{
    public class ReferenceLineHandler : BaseLineHandler
    {
        private readonly ObservableCollection<TextEditorViewModel> _textEditors;
        private readonly BaseLineHandler _lineHandlerChain;

        public ReferenceLineHandler(ObservableCollection<TextEditorViewModel> textEditors, BaseLineHandler lineHandlerChain)
        {
            _textEditors = textEditors;
            _lineHandlerChain = lineHandlerChain;
        }

        public override bool CanHandle(string line)
        {
            return int.TryParse(line, out int number) && number >= 1 && number <= 9;
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                int index = int.Parse(line) - 1;

                if (index < _textEditors.Count)
                {
                    var referencedEditor = _textEditors[index];
                    var referencedContent = referencedEditor.Content ?? string.Empty;

                    if (referencedEditor.IsPlainTextMode)
                    {
                        return referencedContent;
                    }
                    else
                    {
                        return ProcessReferencedContent(referencedContent);
                    }
                }
                else
                {
                    return $"Error: Window {line} does not exist.";
                }
            }

            return PassToNext(line);
        }

        private string ProcessReferencedContent(string referencedContent)
        {
            var result = new StringBuilder();
            var lines = referencedContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var referencedLine in lines)
            {
                result.AppendLine(_lineHandlerChain.Handle(referencedLine));
            }

            return result.ToString();
        }
    }
}
