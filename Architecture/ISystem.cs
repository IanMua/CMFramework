using CMUFramework_Embark.Architecture.Rule;

namespace CMUFramework_Embark.Architecture
{
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanGetModel, ICanSendEvent,
        ICanRegisterEvent
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
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

        void ISystem.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }
}