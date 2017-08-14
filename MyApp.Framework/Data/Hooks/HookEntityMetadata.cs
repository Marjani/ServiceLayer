using System.Data.Entity;

namespace MyApp.Framework.Data.Hooks
{
    public class HookEntityMetadata
    {
        private EntityState _state;

        public HookEntityMetadata(EntityState state)
        {
            _state = state;
        }

        public EntityState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;

                _state = value;
                HasStateChanged = true;
            }
        }

        public bool HasStateChanged { get; private set; }
    }
}