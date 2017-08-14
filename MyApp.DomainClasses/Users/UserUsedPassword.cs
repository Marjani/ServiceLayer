using MyApp.Framework.Domain.Entities;

namespace MyApp.DomainClasses.Users
{
    public class UserUsedPassword : Entity
    {
        #region Properties
        public string HashedPassword { get; set; }

        #endregion

        #region Navigation Properties

        public User User { get; set; }
        public long UserId { get; set; }

        #endregion
    }
}