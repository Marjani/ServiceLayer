using MyApp.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.DomainClasses.Users;
using System.Threading;
using MyApp.Framework.Application.Services;

namespace MyApp.ServiceLayer.Users
{
    public interface IUserRepository : IIdentityRepository<User>
    {
        User FindByUserName(string username);
        Task<User> FindByUserNameAsync(string username);
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username);
    }
}
