using CopyChanges.Commands;
using CopyChanges.LineHandlers;
using System.Windows.Input;
using System.Text;
using CopyChanges.Interfaces;

namespace CopyChanges.ViewModels
{
    public class TextEditorViewModel : BaseViewModel
    {
        private string _content;
        private BaseLineHandler _lineHandlerChain;
        private readonly IClipboardService _clipboardService;

        public int EditorNumber { get; set; }

        public string Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand CopyContentCommand { get; }

        public TextEditorViewModel(BaseLineHandler lineHandlerChain, IClipboardService clipboardService, int editorNumber)
        {
            _lineHandlerChain = lineHandlerChain;
            _clipboardService = clipboardService;
            EditorNumber = editorNumber;
            CopyContentCommand = new RelayCommand(CopyContent);
        }

        public void UpdateLineHandlerChain(BaseLineHandler newChain)
        {
            _lineHandlerChain = newChain;
        }

        private void CopyContent(object parameter)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                var lines = Content.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
                var result = new StringBuilder();

                foreach (var line in lines)
                {
                    var processedLine = _lineHandlerChain.Handle(line);
                    result.AppendLine(processedLine); // Append each line, including empty ones
                }

                _clipboardService.SetText(result.ToString());
            }
        }
    }
}