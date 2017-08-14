using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public abstract class DeletionTrackingEntity : Entity, IDeletionTracking
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletionDateTime { get; set; }
        public string DeleterIp { get; set; }
        public string DeleterBrowserName { get; set; }
        public long? DeleterUserId { get; set; }
    }

    public abstract class DeletionTrackingEntity<TUser> : DeletionTrackingEntity, IDeletionTracking<TUser>
        where TUser : Entity
    {
        public TUser DeleterUser { get; set; }
    }
}