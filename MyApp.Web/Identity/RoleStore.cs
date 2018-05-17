using Microsoft.AspNet.Identity;
using MyApp.DomainClasses.Roles;
using MyApp.Framework.Data;
using MyApp.ServiceLayer.ExternalLogins;
using MyApp.ServiceLayer.Roles;
using MyApp.ServiceLayer.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Web.Identity
{
    public class RoleStore : IRoleStore<IdentityRole, long>, IQueryableRoleStore<IdentityRole, long>, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        IUserRepository _userRepository;
        IRoleRepository _roleRepository;
        IExternalLoginRepository _externalLoginRepository;

        public RoleStore(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IExternalLoginRepository externalLoginRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _externalLoginRepository = externalLoginRepository;
        }

        #region IRoleStore<IdentityRole, long> Members
        public System.Threading.Tasks.Task CreateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            var r = getRole(role);

            _roleRepository.Add(r);
            return _unitOfWork.SaveChangesAsync();
        }

        public System.Threading.Tasks.Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            var r = getRole(role);

           _roleRepository.Remove(r);
            return _unitOfWork.SaveChangesAsync();
        }

        public System.Threading.Tasks.Task<IdentityRole> FindByIdAsync(long roleId)
        {
            var role = _roleRepository.FindById(roleId);
            return Task.FromResult<IdentityRole>(getIdentityRole(role));
        }

        public System.Threading.Tasks.Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var role = _roleRepository.FindByName(roleName);
            return Task.FromResult<IdentityRole>(getIdentityRole(role));
        }

        public System.Threading.Tasks.Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            var r = getRole(role);
            _roleRepository.Update(r);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }
        #endregion

        #region IQueryableRoleStore<IdentityRole, long> Members
        public IQueryable<IdentityRole> Roles
        {
            get
            {
                return _roleRepository
                    .GetAll()
                    .Select(x => getIdentityRole(x))
                    .AsQueryable();
            }
        }
        #endregion

        #region Private Methods
        private Role getRole(IdentityRole identityRole)
        {
            if (identityRole == null)
                return null;
            return new Role
            {
                Id = identityRole.Id,
                Name = identityRole.Name
            };
        }

        private IdentityRole getIdentityRole(Role role)
        {
            if (role == null)
                return null;
            return new IdentityRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }
        #endregion
    }
}