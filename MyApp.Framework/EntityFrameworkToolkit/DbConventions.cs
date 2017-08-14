using System.Data.Entity.ModelConfiguration.Conventions;
using MyApp.Framework.Domain.Entities;

namespace MyApp.Framework.EntityFrameworkToolkit
{
    public class DbConventions : Convention
    {
        public DbConventions()
        {
            Properties()
                .Where(p => p.Name == nameof(Entity.RowVersion))
                .Configure(p => p.IsRowVersion());

            Types<Entity>().Configure(x => x.Ignore(y => y.State));
        }
    }
}