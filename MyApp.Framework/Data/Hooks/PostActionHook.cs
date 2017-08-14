using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PostActionHook<TEntity> : IPostActionHook
    {
        public void Hook(object entity, HookEntityMetadata metadata)
        {
            if (typeof(TEntity).IsAssignableFrom(entity.GetType()))
                Hook((TEntity)entity, metadata);
        }

        public abstract EntityState HookState { get; }

        protected abstract void Hook(TEntity entity, HookEntityMetadata metadata);
    }
}