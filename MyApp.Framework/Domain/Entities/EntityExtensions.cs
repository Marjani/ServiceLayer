using System;
using System.Data.Entity.Core.Objects;
using MyApp.Framework.Domain.Entities.Tracking;

namespace MyApp.Framework.Domain.Entities
{
    public static class EntityExtensions
    {
        public static bool IsNullOrDeleted(this ISoftDeletable entity)
        {
            return entity == null || entity.IsDeleted;
        }

        public static void UnDelete(this ISoftDeletable entity)
        {
            entity.IsDeleted = false;

            var deletionAuditedEntity = entity as IDeletionTracking;
            if (deletionAuditedEntity == null) return;

            deletionAuditedEntity.DeletionDateTime = null;
            deletionAuditedEntity.DeleterUserId = null;
            deletionAuditedEntity.DeleterBrowserName = null;
            deletionAuditedEntity.DeleterIp = null;
        }

        public static Type GetRealType(this Entity entity)
        {
            var userType = ObjectContext.GetObjectType(entity.GetType());
            return userType;
        }
    }
}