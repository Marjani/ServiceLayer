using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using MyApp.DomainClasses.Users;

namespace MyApp.DataLayer.Mappings
{
    public class UserLoginMap : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginMap()
        {
            Property(l => l.LoginProvider)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(
                        new IndexAttribute("UIX_User_LoginProvider_ProviderKey_UserId") {Order = 1, IsUnique = true}));

            Property(l => l.ProviderKey)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(
                        new IndexAttribute("UIX_User_LoginProvider_ProviderKey_UserId") {Order = 2, IsUnique = true}));

            Property(l => l.UserId)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(
                        new IndexAttribute("UIX_User_LoginProvider_ProviderKey_UserId") {Order = 3, IsUnique = true}));
        }
    }
}