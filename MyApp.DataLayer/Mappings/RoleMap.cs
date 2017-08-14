using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using MyApp.DomainClasses.Roles;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Data;

namespace MyApp.DataLayer.Mappings
{
    public class RoleMap : TrackableEntityMap<Role, User>
    {
        public RoleMap()
        {
            Property(r => r.Name).IsRequired().HasMaxLength(50);
            Property(r => r.NormalizedName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UIX_Role_NormalizedName") {IsUnique = true}));

            HasMany(r => r.Claims).WithRequired().HasForeignKey(rc => rc.RoleId);
        }
    }
}