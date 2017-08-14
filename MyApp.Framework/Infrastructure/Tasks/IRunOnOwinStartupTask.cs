namespace MyApp.Framework.Infrastructure.Tasks
{
    public interface IRunOnOwinStartupTask
    {
        int Order { get; }
        void Execute();
    }
}