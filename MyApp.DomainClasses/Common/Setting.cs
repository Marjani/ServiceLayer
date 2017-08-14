using MyApp.DomainClasses.Users;
using MyApp.Framework.Domain.Entities;
using MyApp.Framework.Domain.Entities.Tracking;

namespace MyApp.DomainClasses.Common
{
    public class Setting : ModificationTrackingEntity, ISystemDefaultEntry
    {
        #region Properties
        public string Name { get; set; }
        public string Value { get; set; }
        public SettingScopes Scopes { get; set; }
        public bool IsSystemEntry { get; set; }

        #endregion

        #region Navigation Properties
        public long? UserId { get; set; }
        public User User { get; set; }

        #endregion

    }
}
