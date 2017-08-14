using System.Data.Entity;
using MyApp.Framework.Dependency;

namespace MyApp.Framework.Data.Hooks
{
    public interface IHook : ITransientDependency
    {
        EntityState HookState { get; }
        void Hook(object entity, HookEntityMetadata metadata);
    }
}