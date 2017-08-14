namespace MyApp.Framework.Domain.Entities
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}