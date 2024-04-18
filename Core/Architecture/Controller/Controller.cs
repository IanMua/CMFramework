namespace CMFramework
{
    public interface IController : ICanSendCommand, ICanGetSystem, ICanGetModel,
        ICanRegisterEvent, ICanSendQuery
    {
    }
}