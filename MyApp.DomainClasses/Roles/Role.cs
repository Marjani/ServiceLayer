using System.Collections.Generic;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Domain.Entities;
using MyApp.Framework.Domain.Entities.Tracking;

namespace MyApp.DomainClasses.Roles
{
    public class Role : TrackableEntity<User>, ISystemDefaultEntry, IActivatable
    {
        #region Constructor

        public Role()
        {
            Claims = new HashSet<RoleClaim>();
            Users = new HashSet<User>();
        }

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSystemEntry { get; set; }

        #endregion

        #region Navigation Properties

        public ICollection<RoleClaim> Claims { get; set; }
        public ICollection<User> Users { get; set; }

        #endregion
    }
}