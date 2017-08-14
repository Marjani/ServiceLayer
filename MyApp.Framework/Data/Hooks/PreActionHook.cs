using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public abstract class PreActionHook<TEntity> : IPreActionHook
    {
        public virtual bool RequiresValidation => false;
        public abstract EntityState HookState { get; }

        public void Hook(object entity, HookEntityMetadata metadata)
        {
            if (typeof(TEntity).IsAssignableFrom(entity.GetType()))
                Hook((TEntity)entity, metadata);
        }

        protected abstract void Hook(TEntity entity, HookEntityMetadata metadata);
    }
}