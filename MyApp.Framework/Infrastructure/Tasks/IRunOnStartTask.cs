namespace MyApp.Framework.Infrastructure.Tasks
{
    public interface IRunOnStartTask
    {
        int Order { get; }
        void Execute();
    }
}