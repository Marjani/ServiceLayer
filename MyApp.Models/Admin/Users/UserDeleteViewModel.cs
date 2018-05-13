using MyApp.Framework.Application.Models;

namespace MyApp.Models.Admin.Users
{
    public class UserDeleteViewModel : IDeleteModel
    {
        public long Id { get; set; }

        public byte[] RowVersion { get; set; }
    }
}