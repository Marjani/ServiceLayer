using System;

namespace MyApp.Framework.Domain.Entities.Tracking
{
    public interface IHasDeletionDateTime : ISoftDeletable
    {
        DateTimeOffset? DeletionDateTime { get; set; }
    }
}