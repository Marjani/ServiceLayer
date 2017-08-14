﻿namespace MyApp.Framework.Domain.Entities.Tracking
{
    public interface ICreationTracking : IHasCreationDateTime
    {
        string CreatorIp { get; set; }
        string CreatorBrowserName { get; set; }
        long? CreatorUserId { get; set; }
    }

    public interface ICreationTracking<TUser> : ICreationTracking
        where TUser : Entity
    {
        TUser CreatorUser { get; set; }
    }
}