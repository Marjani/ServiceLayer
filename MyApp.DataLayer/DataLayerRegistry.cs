using MyApp.DataLayer.Context;
using MyApp.Framework.Data;
using MyApp.Framework.Infrastructure.Tasks;
using StructureMap;

namespace MyApp.DataLayer
{
    public class DataLayerRegistry : Registry
    {
        public DataLayerRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.AssembliesFromApplicationBaseDirectory();
                scan.AddAllTypesOf<IRunOnStartTask>();
            });

            //todo:use container per request (Nested Containers) instead of HttpContextLifeCycle
            For<IUnitOfWork>().Use<ApplicationDbContext>();
        }
    }
}