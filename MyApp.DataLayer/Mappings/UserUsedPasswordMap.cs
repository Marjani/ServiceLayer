using System.Data.Entity.ModelConfiguration;
using MyApp.DomainClasses.Users;

namespace MyApp.DataLayer.Mappings
{
    public class UserUsedPasswordMap : EntityTypeConfiguration<UserUsedPassword>
    {
        public UserUsedPasswordMap()
        {
            Property(a => a.HashedPassword).HasMaxLength(256).IsRequired();
        }
    }
}