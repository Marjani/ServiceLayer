using MyApp.Framework.Application.Models;

namespace MyApp.Models.Admin.Roles
{
    public class RoleDeleteViewModel : IDeleteModel
    {
        public long Id { get; set; }

        public byte[] RowVersion { get; set; }
    }
}