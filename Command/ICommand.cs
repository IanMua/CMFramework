namespace CMUFramework_Embark.Command
{
    public interface ICommand
    {
        void Execute();

        void Undo();
    }
}