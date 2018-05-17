using MyApp.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.DomainClasses.ExternalLogins;
using System.Threading;

namespace MyApp.ServiceLayer.ExternalLogins
{
    public interface IExternalLoginRepository : IIdentityRepository<ExternalLogin>
    {
        ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey);
        Task<ExternalLogin> GetByProviderAndKeyAsync(CancellationToken cancellationToken, string loginProvider, string providerKey);
    }
}
