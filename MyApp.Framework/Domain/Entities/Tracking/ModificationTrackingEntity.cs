using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public abstract class ModificationTrackingEntity : Entity, IModificationTracking
    {
        public DateTimeOffset? LastModificationDateTime { get; set; }
        public string LastModifierIp { get; set; }
        public string LastModifierBrowserName { get; set; }
        public long? LastModifierUserId { get; set; }
    }

    public abstract class ModificationTrackingEntity<TUser> : ModificationTrackingEntity, IModificationTracking<TUser>
        where TUser : Entity
    {
        public TUser LastModifierUser { get; set; }
    }
}