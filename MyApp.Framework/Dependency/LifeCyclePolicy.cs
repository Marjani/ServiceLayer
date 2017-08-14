using System;
using StructureMap;
using StructureMap.Pipeline;

namespace MyApp.Framework.Dependency
{
    public class LifeCyclePolicy : IInstancePolicy
    {
        public void Apply(Type pluginType, Instance instance)
        {
            if (typeof(ISingletonDependency).IsAssignableFrom(instance.ReturnedType))
                instance.SetLifecycleTo<SingletonLifecycle>();
            else if (typeof(ITransientDependency).IsAssignableFrom(instance.ReturnedType))
                instance.SetLifecycleTo<TransientLifecycle>();
        }
    }
}