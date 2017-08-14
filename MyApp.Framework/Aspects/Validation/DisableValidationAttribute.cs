using System;

namespace MyApp.Framework.Aspects.Validation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class )]
    public sealed class DisableValidationAttribute : Attribute
    {

    }
}