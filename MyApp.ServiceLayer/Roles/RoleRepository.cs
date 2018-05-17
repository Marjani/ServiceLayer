using AutoMapper;
using MyApp.DataLayer.Context;
using MyApp.DomainClasses.Roles;
using MyApp.Framework.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.ServiceLayer.Roles
{
    internal class RoleRepository : IdentityRepository<Role>, IRoleRepository
    {
        internal RoleRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public Role FindByName(string roleName)
        {
            return Set.FirstOrDefault(x => x.Name == roleName);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return Set.FirstOrDefaultAsync(x => x.Name == roleName);
        }

        public Task<Role> FindByNameAsync(System.Threading.CancellationToken cancellationToken, string roleName)
        {
            return Set.FirstOrDefaultAsync(x => x.Name == roleName, cancellationToken);
        }
    }
}
