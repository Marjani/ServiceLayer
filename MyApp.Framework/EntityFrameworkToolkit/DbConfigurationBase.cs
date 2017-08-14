using System.Data.Entity;
using MyApp.Framework.EntityFrameworkToolkit.Interceptors;

namespace MyApp.Framework.EntityFrameworkToolkit
{
    public abstract class DbConfigurationBase : DbConfiguration
    {
        protected DbConfigurationBase()
        {
            AddInterceptor(new YeKeInterceptor());
        }
    }
}