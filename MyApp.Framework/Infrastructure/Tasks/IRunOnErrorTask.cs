namespace MyApp.Framework.Infrastructure.Tasks
{
    public interface IRunOnErrorTask
    {
        int Order { get; }
        void Execute();
    }
}