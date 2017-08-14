namespace MyApp.Framework.Infrastructure.Tasks
{
    public interface IRunOnBeginRequestTask
    {
        int Order { get; }
        void Execute();
    }
}