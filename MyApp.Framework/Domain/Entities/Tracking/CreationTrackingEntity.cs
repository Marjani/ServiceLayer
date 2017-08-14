using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public abstract class CreationTrackingEntity : Entity, ICreationTracking
    {
        public DateTimeOffset CreationDateTime { get; set; }
        public string CreatorIp { get; set; }
        public string CreatorBrowserName { get; set; }
        public long? CreatorUserId { get; set; }
    }

    public abstract class CreationTrackingEntity<TUser> : CreationTrackingEntity, ICreationTracking<TUser>
        where TUser : Entity
    {
        public TUser CreatorUser { get; set; }
    }
}