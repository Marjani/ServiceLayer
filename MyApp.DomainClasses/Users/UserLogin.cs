using MyApp.Framework.Domain.Entities;

namespace MyApp.DomainClasses.Users
{
    public class UserLogin : Entity
    {
        #region Properties

        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }


        #endregion

        #region Navigation Properties

        public User User { get; set; }
        public long UserId { get; set; }

        #endregion
    }
}