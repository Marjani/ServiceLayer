namespace MyApp.Framework.Infrastructure.Tasks
{
    public interface IRunOnEndTask
    {
        int Order { get; }
        void Execute();
    }
}