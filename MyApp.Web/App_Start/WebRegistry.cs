using MyApp.Framework.Infrastructure.Tasks;
using MyApp.ServiceLayer;
using StructureMap;

namespace MyApp.Web
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.AssembliesFromApplicationBaseDirectory();
                
                scan.AddAllTypesOf<IRunOnEndTask>();
                scan.AddAllTypesOf<IRunOnOwinStartupTask>();
                scan.AddAllTypesOf<IRunOnStartTask>();
                scan.AddAllTypesOf<IRunOnBeginRequestTask>();
                scan.AddAllTypesOf<IRunOnErrorTask>();
                scan.AddAllTypesOf<IRunOnEndRequestTask>();

                scan.Assembly(typeof(ServiceLayerRegistry).Assembly);
                scan.LookForRegistries();
            });
        }
    }
}