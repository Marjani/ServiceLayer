using System.Data.Entity.ModelConfiguration;
using MyApp.Framework.Domain.Entities;
using MyApp.Framework.Domain.Entities.Tracking;

namespace MyApp.Framework.Data
{
    public abstract class TrackableEntityMap<TEntity, TUser> : EntityTypeConfiguration<TEntity>
        where TUser : Entity
        where TEntity : TrackableEntity<TUser>
    {
        protected TrackableEntityMap()
        {
            Property(a => a.CreatorIp).HasMaxLength(255).IsRequired();
            Property(a => a.LastModifierIp).HasMaxLength(255).IsOptional();

            Property(a => a.CreationDateTime).IsRequired();
            Property(a => a.LastModificationDateTime).IsOptional();

            Property(a => a.CreatorBrowserName).HasMaxLength(1024).IsRequired();
            Property(a => a.LastModifierBrowserName).HasMaxLength(1024).IsOptional();

            HasOptional(a => a.CreatorUser).WithMany().HasForeignKey(a => a.CreatorUserId).WillCascadeOnDelete(false);
            HasOptional(a => a.LastModifierUser).WithMany().HasForeignKey(a => a.LastModifierUserId)
                .WillCascadeOnDelete(false);
        }
    }
}