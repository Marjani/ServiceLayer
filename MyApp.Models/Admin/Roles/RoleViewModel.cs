using MyApp.DomainClasses.Roles;
using MyApp.Framework.Application.Models;
using MyApp.Framework.MapperToolkit;

namespace MyApp.Models.Admin.Roles
{
    public class RoleViewModel : IModel, IMapFrom<Role>
    {
        public long Id { get; set; }
    }
}