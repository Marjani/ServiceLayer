using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public abstract class FullTrackableEntity : Entity, IFullTrackable
    {
        public DateTimeOffset CreationDateTime { get; set; }
        public DateTimeOffset? LastModificationDateTime { get; set; }
        public DateTimeOffset? DeletionDateTime { get; set; }
        public string CreatorIp { get; set; }
        public string LastModifierIp { get; set; }
        public string DeleterIp { get; set; }
        public string CreatorBrowserName { get; set; }
        public string LastModifierBrowserName { get; set; }
        public string DeleterBrowserName { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? CreatorUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public abstract class FullTrackableEntity<TUser> : FullTrackableEntity, IFullTrackable<TUser>
        where TUser : Entity
    {
        public TUser CreatorUser { get; set; }
        public TUser LastModifierUser { get; set; }
        public TUser DeleterUser { get; set; }
    }
}