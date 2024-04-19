namespace CMFramework
{
    public interface IQuery<TResult> : ICanSetArchitecture, ICanGetModel, ICanGetSystem,
        ICanSendQuery
    {
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        private IArchitecture _architecture;

        T IQuery<T>.Do()
        {
            return OnDo();
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        protected abstract T OnDo();
    }
}