using MyApp.Framework.Domain.Entities;

namespace MyApp.DomainClasses.Users
{
    public class UserToken : Entity, IHasRowLevelSecurity
    {
        #region Properties

        #endregion

        #region Navigation Properties
        public long UserId { get; set; }

        #endregion

    }
}
