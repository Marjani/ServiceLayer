using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PostInsertHook<TEntity> : PostActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Added;
    }
}