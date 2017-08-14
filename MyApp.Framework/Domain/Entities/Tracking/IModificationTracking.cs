﻿namespace MyApp.Framework.Domain.Entities.Tracking
{
    public interface IModificationTracking : IHasModificationDateTime
    {
        string LastModifierIp { get; set; }
        string LastModifierBrowserName { get; set; }
        long? LastModifierUserId { get; set; }
    }

    public interface IModificationTracking<TUser> : IModificationTracking
        where TUser : Entity
    {
        TUser LastModifierUser { get; set; }
    }
}