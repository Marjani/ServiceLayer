using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using MyApp.Framework.Extensions;
using MyApp.Framework.IdentityToolkit.Core;

namespace MyApp.Framework.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        #region Constructor

        public CurrentUserService(IIdentity identity, IPrincipal principal, HttpContextBase context)
        {
            _identity = identity;
            _principal = principal;
            _context = context;
        }

        #endregion

        #region Fields 
        private readonly HttpContextBase _context;

        private readonly IIdentity _identity;
        private readonly IPrincipal _principal;

        #endregion

        #region Properties 
        public string DisplayName => ((ClaimsPrincipal)_principal).FindFirst(ClaimTypes.Surname).Value;
        public bool IsInRole(string roleName)
        {
            return _principal.IsInRole(roleName);
        }

        public long Id => _identity.GetUserId();
        public string UserName => _identity.Name;
        public bool IsAuthenticated => _identity != null && _identity.IsAuthenticated;
        public string BrowserName => _context.Request.GetBrowser();
        public string Ip => _context.Request.GetUserIp();

        #endregion
    }
}