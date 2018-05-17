using AutoMapper;
using MyApp.DataLayer.Context;
using MyApp.DomainClasses.ExternalLogins;
using MyApp.Framework.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.ServiceLayer.ExternalLogins
{
    internal class ExternalLoginRepository : IdentityRepository<ExternalLogin>, IExternalLoginRepository
    {
        internal ExternalLoginRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey)
        {
            return Set.FirstOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
        }

        public Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey)
        {
            return Set.FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey, cancellationToken);
        }
    }
}
