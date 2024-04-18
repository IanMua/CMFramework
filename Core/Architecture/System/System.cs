namespace CMFramework
{
    public interface ISystem : ICanSetArchitecture, ICanGetUtility, ICanGetModel, ICanSendEvent,
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

        /// <summary>
        /// 执行初始化
        /// </summary>
        protected abstract void OnInit();
    }
}