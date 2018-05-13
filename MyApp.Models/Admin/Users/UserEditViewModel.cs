using MyApp.DomainClasses.Users;
using MyApp.Framework.Application.Models;
using MyApp.Framework.MapperToolkit;

namespace MyApp.Models.Admin.Users
{
    public class UserEditViewModel : IEditModel, IMapFrom<MyApp.DomainClasses.Users.User>
    {
        public string Email { get; set; }
        public long Id { get; set; }
        public string PasswordHash { get; set; }
        public byte[] RowVersion { get; set; }
        public string SecurityStamp { get; set; }
        public string UserName { get; set; }
    }
}