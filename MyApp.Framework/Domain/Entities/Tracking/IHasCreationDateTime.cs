using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public interface IHasCreationDateTime
    {
        DateTimeOffset CreationDateTime { get; set; }
    }
}