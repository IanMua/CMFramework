using CMUFramework_Embark.Architecture.Rule;

namespace CMUFramework_Embark.Architecture
{
    public interface IController : IBelongToArchitecture, ICanSendCommand, ICanGetSystem, ICanGetModel,
        ICanRegisterEvent
    {
    }
}