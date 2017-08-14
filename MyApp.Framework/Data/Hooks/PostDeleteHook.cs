using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PostDeleteHook<TEntity> : PostActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Deleted;
    }
}