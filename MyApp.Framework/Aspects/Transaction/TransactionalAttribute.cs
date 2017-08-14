using System;

namespace MyApp.Framework.Aspects.Transaction
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TransactionalAttribute : Attribute
    {

    }
}
