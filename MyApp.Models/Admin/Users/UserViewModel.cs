using MyApp.DomainClasses.Users;
using MyApp.Framework.Application.Models;
using MyApp.Framework.MapperToolkit;

namespace MyApp.Models.Admin.Users
{
    public class UserViewModel : IModel, IMapFrom<MyApp.DomainClasses.Users.User>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
    }
}