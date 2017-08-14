using System.Collections.Generic;
using MyApp.DomainClasses.Roles;
using MyApp.Framework.Application.Models;
using MyApp.Framework.MapperToolkit;

namespace MyApp.Models.Admin.Roles
{
    public class RoleCreateViewModel : IMapFrom<Role>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public IList<LookupItem> Claims { get; set; }
    }
}