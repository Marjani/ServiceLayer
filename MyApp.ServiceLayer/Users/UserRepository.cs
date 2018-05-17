using AutoMapper;
using MyApp.DataLayer.Context;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.ServiceLayer.Users
{
    internal class UserRepository : IdentityRepository<User>, IUserRepository
    {
        internal UserRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public User FindByUserName(string username)
        {
            return Set.FirstOrDefault(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(System.Threading.CancellationToken cancellationToken, string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }
    }
}
