using System.Linq;
using FluentValidation;
using StructureMap.DynamicInterception;

namespace MyApp.Framework.Aspects.Validation
{
    public class ValidatorInterceptor : ISyncInterceptionBehavior
    {
        private readonly IValidatorFactory _validatorFactory;

        public ValidatorInterceptor(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public IMethodInvocationResult Intercept(ISyncMethodInvocation methodInvocation)
        {
            var argumentValues = methodInvocation.Arguments.Select(a => a.Value).ToArray();

            var validator = new MethodInvocationValidator(_validatorFactory, methodInvocation.MethodInfo,
                argumentValues);

            validator.Validate();

            return methodInvocation.InvokeNext();
        }
    }
}