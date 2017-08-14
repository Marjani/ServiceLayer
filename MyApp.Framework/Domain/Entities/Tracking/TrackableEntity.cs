using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public abstract class TrackableEntity : Entity, ITrackable
    {
        public DateTimeOffset CreationDateTime { get; set; }
        public DateTimeOffset? LastModificationDateTime { get; set; }
        public string CreatorIp { get; set; }
        public string LastModifierIp { get; set; }
        public string CreatorBrowserName { get; set; }
        public string LastModifierBrowserName { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? CreatorUserId { get; set; }
    }

    public abstract class TrackableEntity<TUser> : TrackableEntity, ITrackable<TUser>
        where TUser : Entity
    {
        public TUser CreatorUser { get; set; }
        public TUser LastModifierUser { get; set; }
    }
}