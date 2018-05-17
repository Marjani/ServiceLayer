using MyApp.DomainClasses.Users;
using MyApp.Framework.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DomainClasses.ExternalLogins
{
    public class ExternalLogin:Entity
    {
        private User _user;

        #region Scalar Properties
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual long UserId { get; set; }
        #endregion

        #region Navigation Properties
        public virtual User User
        {
            get { return _user; }
            set
            {
                _user = value;
                UserId = value.Id;
            }
        }
        #endregion
    }
}
