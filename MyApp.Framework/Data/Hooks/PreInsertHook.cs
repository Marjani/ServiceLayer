using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PreInsertHook<TEntity> : PreActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Added;
    }
}