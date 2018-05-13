using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public IdentityRole(string name, long id)
        {
            this.Name = name;
            this.Id = id;
        }

        public long Id { get; set; }
        public string Name { get; set; }
    }
}