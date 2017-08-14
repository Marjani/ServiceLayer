using System.Data.Entity;
using MyApp.Framework.Domain.Entities;

namespace MyApp.Framework.Data.Hooks
{
    public class HookedEntityEntry
    {
        public Entity Entity { get; set; }
        public EntityState PreSaveState { get; set; }
    }
}