using System;

namespace MyApp.Framework.Aspects.Validation
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ValidateWithRuleAttribute : Attribute
    {
        public ValidateWithRuleAttribute(params string[] ruleSets)
        {
            RuleSetNames = ruleSets;
        }

        public string[] RuleSetNames { get; }
    }
}
