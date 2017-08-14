using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PreUpdateHook<TEntity> : PreActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Modified;
    }
}