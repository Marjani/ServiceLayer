using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PreDeleteHook<TEntity> : PreActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Deleted;
    }
}