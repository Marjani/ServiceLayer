namespace MyApp.Framework.Application.Models
{
    public interface IEditModel : IModel
    {
        byte[] RowVersion { get; set; }
    }
}