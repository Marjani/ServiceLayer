using MyApp.Framework.Infrastructure.Tasks;
using StructureMap;

namespace MyApp.Framework.FluentValidationToolkit
{
    public class ConfigureFluentValidationTask : IRunOnOwinStartupTask
    {
        private readonly IContainer _container;

        public ConfigureFluentValidationTask(IContainer container)
        {
            _container = container;
        }

        public int Order => 0;
        public void Execute()
        {
            //DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            //FluentValidationModelValidatorProvider.Configure(
            //    provider =>
            //    {
            //        provider.AddImplicitRequiredValidator = false;
            //        provider.ValidatorFactory = _container.GetInstance<StructureMapValidatorFactory>();
            //    });
        }
    }
}