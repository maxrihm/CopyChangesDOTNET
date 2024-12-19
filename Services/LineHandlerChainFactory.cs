using System.Collections.ObjectModel;
using CopyChanges.Interfaces;
using CopyChanges.LineHandlers;
using CopyChanges.ViewModels;

namespace CopyChanges.Services
{
    public class LineHandlerChainFactory : ILineHandlerChainFactory
    {
        private readonly IFileService _fileService;
        private readonly IJsonService _jsonService;

        public LineHandlerChainFactory(IFileService fileService, IJsonService jsonService)
        {
            _fileService = fileService;
            _jsonService = jsonService;
        }

        public BaseLineHandler CreateChain(string projectDirectory, ObservableCollection<TextEditorViewModel> textEditors)
        {
            var vscodeExtensionAllHandler = new VSCodeExtensionAllHandler(_jsonService, projectDirectory);
            var fileLineHandler = new FileLineHandler(_fileService, projectDirectory);
            var referenceLineHandler = new ReferenceLineHandler(textEditors, vscodeExtensionAllHandler);
            var textLineHandler = new TextLineHandler();

            vscodeExtensionAllHandler.SetNext(fileLineHandler);
            fileLineHandler.SetNext(referenceLineHandler);
            referenceLineHandler.SetNext(textLineHandler);

            return vscodeExtensionAllHandler;
        }
    }
}
