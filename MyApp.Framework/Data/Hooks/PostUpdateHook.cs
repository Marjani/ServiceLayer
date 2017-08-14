using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PostUpdateHook<TEntity> : PostActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Modified;
    }
}