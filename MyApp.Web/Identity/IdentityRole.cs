using Microsoft.AspNet.Identity;
using System;

namespace MyApp.Web.Identity
{
    public class IdentityRole : IRole<long>
    {
        public IdentityRole()
        {
        }

        public IdentityRole(string name)
            : this()
        {
            this.Name = name;
        }

        public IdentityRole(string name, Guid id)
        {
            this.Name = name;
            this.Id = id;
        }

        public long Id { get; set; }
        public string Name { get; set; }
    }
}