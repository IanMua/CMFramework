namespace CMFramework
{
    public interface ICommand : ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanGetSystem,
        ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        void Execute();

        void Undo();
    }

    public interface ICommand<T> : ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanGetSystem,
        ICanSendCommand, ICanSendEvent, ICanSendQuery
    {
        T Execute();

        void Undo();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        void ICommand.Execute()
        {
            OnExecute();
        }

        void ICommand.Undo()
        {
            OnUndo();
        }

        protected abstract void OnExecute();
        protected abstract void OnUndo();
    }

    public abstract class AbstractCommand<T> : ICommand<T>
    {
        private IArchitecture _architecture;

        public IArchitecture GetArchitecture()
        {
            return _architecture;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        T ICommand<T>.Execute()
        {
            return OnExecute();
        }

        void ICommand<T>.Undo()
        {
            OnUndo();
        }

        protected abstract T OnExecute();
        protected abstract void OnUndo();
    }
}