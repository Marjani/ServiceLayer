using System.Linq;
using MyApp.Framework.Infrastructure.Tasks;

namespace MyApp.Framework.Infrastructure
{
    public static class Bootstrapper
    {
        public static void RunOnOwinStartup()
        {
            var tasksToRun = IoC.GetAllInstances<IRunOnOwinStartupTask>().OrderBy(a => a.Order);

            foreach (var item in tasksToRun)
                item.Execute();
        }

        public static void RunOnStart()
        {
            var tasksToRun = IoC.GetAllInstances<IRunOnStartTask>().OrderBy(a => a.Order);

            foreach (var item in tasksToRun)
                item.Execute();
        }

        public static void RunOnEnd()
        {
            var tasksToRun = IoC.GetAllInstances<IRunOnEndTask>().OrderBy(a => a.Order);

            foreach (var item in tasksToRun)
                item.Execute();
        }

        public static void RunOnError()
        {
            var tasksToRun = IoC.GetAllInstances<IRunOnErrorTask>().OrderBy(a => a.Order);

            foreach (var item in tasksToRun)
                item.Execute();
        }

        public static void RunOnBeginRequest()
        {
            var tasksToRun = IoC.GetAllInstances<IRunOnBeginRequestTask>().OrderBy(a => a.Order);

            foreach (var item in tasksToRun)
                item.Execute();
        }

        public static void RunOnEndRequest()
        {
            var tasksToRun = IoC.GetAllInstances<IRunOnEndRequestTask>().OrderBy(a => a.Order);

            foreach (var item in tasksToRun)
                item.Execute();
        }
    }
}