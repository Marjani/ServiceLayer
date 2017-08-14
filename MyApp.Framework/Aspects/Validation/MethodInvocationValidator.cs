using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using MyApp.Framework.Extensions;
using MyApp.Framework.GuardToolkit;
using MyApp.Framework.Reflection;

namespace MyApp.Framework.Aspects.Validation
{
    internal class MethodInvocationValidator
    {
        #region Constructor

        public MethodInvocationValidator(IValidatorFactory validatorFactory, MethodInfo method,
            object[] parameterValues)
        {
            Guard.ArgumentNotNull(method, nameof(method));
            Guard.ArgumentNotNull(parameterValues, nameof(parameterValues));
            Guard.ArgumentNotNull(validatorFactory, nameof(validatorFactory));

            _method = method;
            _parameterValues = parameterValues;
            _validatorFactory = validatorFactory;
            _parameters = method.GetParameters();

            _parametersToBeNormalized = new List<IShouldNormalize>();
        }

        #endregion

        #region Public Methods

        public void Validate()
        {
            if (!CheckShouldBeValidate()) return;

            foreach (var parameterValue in _parameterValues)
                ValidateMethodParameter(parameterValue);

            foreach (var parameterToBeNormalized in _parametersToBeNormalized)
                parameterToBeNormalized.Normalize();
        }

        #endregion

        #region Fields

        private readonly MethodInfo _method;
        private readonly object[] _parameterValues;
        private readonly ParameterInfo[] _parameters;
        private readonly IValidatorFactory _validatorFactory;
        private readonly List<IShouldNormalize> _parametersToBeNormalized;

        #endregion

        #region Private Methods

        private bool CheckShouldBeValidate()
        {
            if (!_method.IsPublic)
                return false;

            if (IsValidationDisabled())
                return false;

            if (_parameters.IsNullOrEmpty())
                return false;

            if (_parameters.Length != _parameterValues.Length)
                throw new Exception("Method parameter count does not match with argument count!");

            return true;
        }

        private bool IsValidationDisabled()
        {
            if (_method.IsDefined(typeof(EnableValidationAttribute), true))
                return false;

            return ReflectionHelper
                       .GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(_method) != null;
        }

        private void ValidateMethodParameter(object parameterValue)
        {
            if (parameterValue == null) return;

            var parameterValueList = parameterValue as IEnumerable<object>;
            if (parameterValueList != null)
            {
                var valueList = parameterValueList.ToList();

                ValidateMethodParameterValues(valueList);
            }
            else
            {
                ValidateMethodParameterValues(new List<object> { parameterValue });
            }

            if (parameterValue is IShouldNormalize)
                _parametersToBeNormalized.Add(parameterValue as IShouldNormalize);
        }

        private void ValidateMethodParameterValues(List<object> valueList)
        {
            var ruleSet = GetRuleSet(_method);

            var validator = _validatorFactory.GetValidator(valueList.First().GetType());
            if (validator == null) return;

            foreach (var item in valueList)
                ValidateWithReflection(validator, item, ruleSet);
        }

        private static string GetRuleSet(MemberInfo method)
        {
            const string @default = "default";

            var attribute = method.GetCustomAttribute<ValidateWithRuleAttribute>();

            if (attribute == null)
                return @default;

            var rules = new List<string> { @default };

            rules.AddRange(attribute.RuleSetNames);

            return string.Join(",", rules).TrimEnd(',');
        }

        private static void ValidateAndThrow<T>(IValidator<T> validator, T argument, string ruleSet)
        {
            validator.ValidateAndThrow(argument, ruleSet);
        }

        private void ValidateWithReflection(IValidator validator, object argument, string ruleSet)
        {
            GetType().GetMethod(nameof(ValidateAndThrow), BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(argument.GetType())
                .Invoke(null, new[] { validator, argument, ruleSet });
        }

        #endregion
    }
}