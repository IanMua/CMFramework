using CMUFramework_Embark.Architecture;
using CMUFramework_Embark.Architecture.Rule;

namespace CMUFramework_Embark.Command
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanGetSystem,
        ICanSendCommand, ICanSendEvent
    {
        void Execute();

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
}