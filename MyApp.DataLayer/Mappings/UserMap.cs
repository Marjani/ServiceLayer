using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Data;

namespace MyApp.DataLayer.Mappings
{
    public class UserMap : TrackableEntityMap<User, User>
    {
        public UserMap()
        {
            Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();
            Property(u => u.LastName).HasMaxLength(50).IsOptional();
            Property(u => u.FirstName).IsOptional().HasMaxLength(50);
            Property(u => u.PhoneNumber).IsOptional().HasMaxLength(20);
            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);
            Property(u => u.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UIX_User_NormalizedUserName") { IsUnique = true }));
            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
            Property(u => u.NormalizedEmail)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UIX_User_NormalizedEmail") { IsUnique = true }));

            HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            HasMany(u => u.UsedPasswords).WithRequired().HasForeignKey(up => up.UserId);
            HasMany(u => u.Roles).WithMany(r => r.Users).Map(m =>
            {
                m.MapLeftKey("RoleId");
                m.MapRightKey("UserId");
                m.ToTable("UserRole");
            });
        }
    }
}