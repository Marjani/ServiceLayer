using System.Linq;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MyApp.DataLayer;
using MyApp.Framework.Infrastructure.Tasks;
using MyApp.Framework.MapperToolkit;
using MyApp.Models.Admin.Roles;
using StructureMap;

namespace MyApp.ServiceLayer
{
    public class ServiceLayerRegistry : Registry
    {
        #region Constructor

        public ServiceLayerRegistry()
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

                scan.Assembly(typeof(DataLayerRegistry).Assembly);
                scan.LookForRegistries();

                scan.AddAllTypesOf<Profile>().NameBy(item => item.FullName);
                scan.AddAllTypesOf<IHaveCustomMappings>().NameBy(item => item.FullName);
            });

            FluentValidationConfig();
            AutoMapperConfig();
        }

        #endregion

        #region Private Methods

        private void AutoMapperConfig()
        {
            For<MapperConfiguration>().Singleton().Use("MapperConfig", ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMissingTypeMaps = true;
                    AddProfiles(ctx, cfg);
                    AddIHaveCustomMappings(ctx, cfg);
                    AddMapFrom(cfg);
                });

                config.AssertConfigurationIsValid();

                return config;
            });

            For<IMapper>().Singleton().Use(ctx => ctx.GetInstance<MapperConfiguration>().CreateMapper(ctx.GetInstance));
        }

        private void FluentValidationConfig()
        {
            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ForEach(result =>
                {
                    For(result.InterfaceType)
                        .Singleton()
                        .Use(result.ValidatorType);
                });
        }

        private static void AddMapFrom(IProfileExpression cfg)
        {
            var types = typeof(RoleViewModel).Assembly.GetExportedTypes();
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) && !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Source = i.GetGenericArguments()[0],
                            Destination = t
                        }).ToArray();

            foreach (var map in maps)
                cfg.CreateMap(map.Source, map.Destination);
        }

        private static void AddProfiles(IContext ctx, IMapperConfigurationExpression cfg)
        {
            var profiles = ctx.GetAllInstances<Profile>().ToList();
            foreach (var profile in profiles)
                cfg.AddProfile(profile);
        }

        private static void AddIHaveCustomMappings(IContext ctx, IMapperConfigurationExpression cfg)
        {
            var mappings = ctx.GetAllInstances<IHaveCustomMappings>().ToList();
            foreach (var mapping in mappings)
                mapping.CreateMappings(cfg);
        }

        #endregion
    }
}