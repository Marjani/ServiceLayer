using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public interface IHasModificationDateTime
    {
        DateTimeOffset? LastModificationDateTime { get; set; }
    }
}