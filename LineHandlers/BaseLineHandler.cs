namespace CopyChanges.LineHandlers
{
    public abstract class BaseLineHandler
    {
        protected BaseLineHandler NextHandler;

                protected bool IsPlainTextMode { get; private set; }

        public void SetNext(BaseLineHandler nextHandler)
        {
            NextHandler = nextHandler;
        }

        public abstract bool CanHandle(string line);

        public abstract string Handle(string line);

        protected string PassToNext(string line)
        {
            return NextHandler != null ? NextHandler.Handle(line) : line;
        }
        public void SetIsPlainTextMode(bool isPlainText)
        {
            IsPlainTextMode = isPlainText;
            NextHandler?.SetIsPlainTextMode(isPlainText);
        }
    }
}
