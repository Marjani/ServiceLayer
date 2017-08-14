using System.Data.Entity.ModelConfiguration;
using MyApp.DomainClasses.Roles;

namespace MyApp.DataLayer.Mappings
{
    public class RoleClaimMap : EntityTypeConfiguration<RoleClaim>
    {
        public RoleClaimMap()
        {
            Property(r => r.ClaimType).HasMaxLength(256).IsRequired();
            Property(r => r.ClaimValue).HasMaxLength(256).IsRequired();
        }
    }
}