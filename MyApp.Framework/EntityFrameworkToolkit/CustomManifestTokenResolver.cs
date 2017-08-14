using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace MyApp.Framework.EntityFrameworkToolkit
{
    public class CustomManifestTokenResolver : IManifestTokenResolver
    {
        public string ResolveManifestToken(DbConnection connection)
        {
            return "2014";
        }
    }
}