using System.Data.Entity.ModelConfiguration;
using MyApp.DomainClasses.Users;

namespace MyApp.DataLayer.Mappings
{
    public class UserClaimMap : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimMap()
        {
            Property(r => r.ClaimType).HasMaxLength(256).IsRequired();
            Property(r => r.ClaimValue).HasMaxLength(256).IsRequired();
        }
    }
}