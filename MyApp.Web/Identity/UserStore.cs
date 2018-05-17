using Microsoft.AspNet.Identity;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Data;
using MyApp.ServiceLayer.ExternalLogins;
using MyApp.ServiceLayer.Roles;
using MyApp.ServiceLayer.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using MyApp.DomainClasses.ExternalLogins;

namespace MyApp.Web.Identity
{

    public class UserStore : IUserLoginStore<IdentityUser, long>, IUserClaimStore<IdentityUser, long>, IUserRoleStore<IdentityUser, long>, IUserPasswordStore<IdentityUser, long>, IUserSecurityStampStore<IdentityUser, long>, IUserStore<IdentityUser, long>, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        IUserRepository _userRepository;
        IRoleRepository _roleRepository;
        IExternalLoginRepository _externalLoginRepository;

        public UserStore(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IExternalLoginRepository externalLoginRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _externalLoginRepository = externalLoginRepository;
        }

        #region IUserStore<IdentityUser, long> Members
        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = getUser(user);

            _userRepository.Add(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = getUser(user);

            _userRepository.Remove(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindByIdAsync(long userId)
        {
            var user = _userRepository.FindById(userId);
            return Task.FromResult<IdentityUser>(getIdentityUser(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = _userRepository.FindByUserName(userName);
            return Task.FromResult<IdentityUser>(getIdentityUser(user));
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentException("user");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            populateUser(u, user);

            _userRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }
        #endregion

        #region IUserClaimStore<IdentityUser, long> Members
        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var c = new UserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);

            _userRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<Claim>>(u.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _userRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IUserLoginStore<IdentityUser, long> Members
        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l = new UserLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = u
            };
            u.Logins.Add(l);

            _userRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var identityUser = default(IdentityUser);

            var l = _externalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = getIdentityUser(l.User);

            return Task.FromResult<IdentityUser>(identityUser);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<UserLoginInfo>>(u.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (login == null)
                throw new ArgumentNullException("login");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var l = u.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);

            _userRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IUserRoleStore<IdentityUser, long> Members
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");
            var r = _roleRepository.FindByName(roleName);
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity.", "roleName");

            u.Roles.Add(r);
            _userRepository.Update(u);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            return Task.FromResult<bool>(u.Roles.Any(x => x.Name == roleName));
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _userRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", "user");

            var r = u.Roles.FirstOrDefault(x => x.Name == roleName);
            u.Roles.Remove(r);

            _userRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IUserPasswordStore<IdentityUser, long> Members
        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserSecurityStampStore<IdentityUser, long> Members
        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        #endregion

        #region Private Methods
        private User getUser(IdentityUser identityUser)
        {
            if (identityUser == null)
                return null;

            var user = new User();
            populateUser(user, identityUser);

            return user;
        }

        private void populateUser(User user, IdentityUser identityUser)
        {
            user.Id = identityUser.Id;
            user.UserName = identityUser.UserName;
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
        }

        private IdentityUser getIdentityUser(User user)
        {
            if (user == null)
                return null;

            var identityUser = new IdentityUser();
            populateIdentityUser(identityUser, user);

            return identityUser;
        }

        private void populateIdentityUser(IdentityUser identityUser, User user)
        {
            identityUser.Id = user.Id;
            identityUser.UserName = user.UserName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
        }
        #endregion
    }
}