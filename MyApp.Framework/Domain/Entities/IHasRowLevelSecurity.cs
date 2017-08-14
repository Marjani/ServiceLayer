namespace MyApp.Framework.Domain.Entities
{
    public interface IHasRowLevelSecurity
    {
        long UserId { get; set; }
    }
}