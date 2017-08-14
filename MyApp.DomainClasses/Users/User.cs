using System;
using System.Collections.Generic;
using MyApp.DomainClasses.Roles;
using MyApp.Framework.Domain.Entities;
using MyApp.Framework.Domain.Entities.Tracking;

namespace MyApp.DomainClasses.Users
{
    public class User : TrackableEntity<User>, ISystemDefaultEntry, IActivatable
    {
        #region Constructor

        public User()
        {
            UsedPasswords = new HashSet<UserUsedPassword>();
            Roles = new HashSet<Role>();
            Logins = new HashSet<UserLogin>();
            Claims = new HashSet<UserClaim>();
            Tokens = new HashSet<UserToken>();
        }

        #endregion

        #region Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEndDateTime { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSystemEntry { get; set; }
        public DateTimeOffset? LastVisitDateTime { get; set; }
        public string PhotoFileName { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public DateTimeOffset RegisterationDateTime { get; set; }
        public DateTimeOffset LastLoggedInDateTime { get; set; }

        #endregion

        #region Navigation Properties

        public ICollection<UserUsedPassword> UsedPasswords { get; set; }
        public ICollection<UserLogin> Logins { get; set; }
        public ICollection<UserClaim> Claims { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<UserToken> Tokens { get; set; }

        #endregion
    }
}