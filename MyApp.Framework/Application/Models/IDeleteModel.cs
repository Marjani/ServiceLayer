namespace MyApp.Framework.Application.Models
{
    public interface IDeleteModel : IModel
    {
        byte[] RowVersion { get; set; }
    }
}
