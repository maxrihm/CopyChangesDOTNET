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
        private readonly IMessageService _messageService;

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
        
        // New Command for clearing content
        public ICommand ClearContentCommand { get; }

        public TextEditorViewModel(BaseLineHandler lineHandlerChain, 
                                   IClipboardService clipboardService, 
                                   IMessageService messageService,
                                   int editorNumber)
        {
            _lineHandlerChain = lineHandlerChain;
            _clipboardService = clipboardService;
            _messageService = messageService;
            EditorNumber = editorNumber;

            CopyContentCommand = new RelayCommand(CopyContent);
            ClearContentCommand = new RelayCommand(ClearContent);
        }

        public void UpdateLineHandlerChain(BaseLineHandler newChain)
        {
            _lineHandlerChain = newChain;
        }

        private void CopyContent(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(Content))
                {
                    _messageService.ShowStatusMessage($"Editor {EditorNumber}: No content to copy.");
                    return;
                }

                var lines = Content.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
                var result = new StringBuilder();

                foreach (var line in lines)
                {
                    var processedLine = _lineHandlerChain.Handle(line);
                    result.AppendLine(processedLine);
                }

                _clipboardService.SetText(result.ToString());
                _messageService.ShowStatusMessage($"Editor {EditorNumber}: Content copied to clipboard.");
            }
            catch (System.Exception ex)
            {
                _messageService.ShowError($"Editor {EditorNumber} copy failed: {ex.Message}");
            }
        }

        private void ClearContent(object parameter)
        {
            Content = string.Empty;
            _messageService.ShowStatusMessage($"Editor {EditorNumber}: Content cleared.");
        }
    }
}