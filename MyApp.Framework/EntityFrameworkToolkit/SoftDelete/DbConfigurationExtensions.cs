using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace MyApp.Framework.EntityFrameworkToolkit.SoftDelete
{
    public static class DbConfigurationExtensions
    {
        public static void AddSoftDeleteInterceptor(this DbConfiguration configuration)
        {
            DbInterception.Add(new SoftDeleteInterceptor());
        }
    }
}