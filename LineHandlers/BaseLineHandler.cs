namespace CopyChanges.LineHandlers
{
    public abstract class BaseLineHandler
    {
        protected BaseLineHandler NextHandler;

        public void SetNext(BaseLineHandler nextHandler)
        {
            NextHandler = nextHandler;
        }

        public abstract bool CanHandle(string line);

        public abstract string Handle(string line);

        protected string PassToNext(string line)
        {
            return NextHandler != null ? NextHandler.Handle(line) : string.Empty;
        }
    }
}
