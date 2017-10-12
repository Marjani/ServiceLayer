using FluentValidation;
using MyApp.Framework.Application.Services;
using MyApp.Framework.Aspects.Transaction;
using MyApp.Framework.Aspects.Validation;
using MyApp.Framework.FluentValidationToolkit;
using MyApp.Framework.Infrastructure.Tasks;
using StructureMap;
using StructureMap.DynamicInterception;

namespace MyApp.Framework
{
    public class FrameworkRegistry : Registry
    {
        public FrameworkRegistry()
        {
            For<IValidatorFactory>().Singleton().Use<StructureMapValidatorFactory>();

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

                scan.LookForRegistries();
            });

            Policies.Interceptors(new DynamicProxyInterceptorPolicy(f => typeof(IApplicationService).IsAssignableFrom(f), typeof(ValidatorInterceptor), typeof(TransactionInterceptor)));
        }
    }
}