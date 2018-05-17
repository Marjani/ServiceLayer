using Microsoft.AspNet.Identity;
using System;

namespace MyApp.Web.Identity
{
    public class IdentityUser : IUser<long>
    {
        public IdentityUser()
        {
        }

        public IdentityUser(string userName)
            : this()
        {
            this.UserName = userName;
        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
    }
}