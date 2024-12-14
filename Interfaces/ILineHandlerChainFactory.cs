using CopyChanges.LineHandlers;
using System.Collections.ObjectModel;
using CopyChanges.ViewModels;

namespace CopyChanges.Interfaces
{
    public interface ILineHandlerChainFactory
    {
        BaseLineHandler CreateChain(string projectDirectory, ObservableCollection<TextEditorViewModel> textEditors);
    }
}
