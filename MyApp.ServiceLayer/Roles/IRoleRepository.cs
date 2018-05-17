using MyApp.DomainClasses.Roles;
using MyApp.Framework.Application.Services;
using MyApp.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.ServiceLayer.Roles
{
    public interface IRoleRepository : IIdentityRepository<Role>
    {
        Role FindByName(string roleName);
        Task<Role> FindByNameAsync(string roleName);
        Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName);
    }
}
