namespace MyApp.Framework.Infrastructure.Tasks
{
    public interface IRunOnEndRequestTask
    {
        int Order { get; }
        void Execute();
    }
}