using MyApp.Framework.Dependency;

namespace MyApp.Framework.Application.Services
{
    public interface ICurrentUserService : ITransientDependency
    {
        long Id { get; }
        string UserName { get; }
        string DisplayName { get; }
        string BrowserName { get; }
        string Ip { get; }
        bool IsAuthenticated { get; }
    }
}