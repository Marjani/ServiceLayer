using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using MyApp.Framework.EntityFrameworkToolkit.SoftDelete;

namespace MyApp.Framework.EntityFrameworkToolkit.Extensions
{
    public static class DbModelBuilderExtensions
    {
        public static void AddSoftDeleteConvention(this DbModelBuilder modelBuilder)
        {
            var conv = new AttributeToTableAnnotationConvention<SoftDeleteAttribute, string>(
                "SoftDeleteColumnName",
                (type, attributes) => attributes.Single().ColumnName);

            modelBuilder.Conventions.Add(conv);
        }

        public static void AddFrameworkConventions(this DbModelBuilder modelBuilder)
        {
            //modelBuilder.AddSoftDeleteConvention();//instead use ISoftDeletable Filter 
            modelBuilder.Conventions.Add<DbConventions>();
        }
    }
}