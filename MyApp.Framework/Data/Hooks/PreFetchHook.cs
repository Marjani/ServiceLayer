using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PreFetchHook<TEntity> : PreActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Unchanged;
    }
}