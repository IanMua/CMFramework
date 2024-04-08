using CMUFramework_Embark.Architecture;
using CMUFramework_Embark.Architecture.Rule;

namespace CMUFramework_Embark.Query
{
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem,
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