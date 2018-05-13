using System.Collections.Generic;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Application.Models;
using MyApp.Framework.MapperToolkit;
using System;

namespace MyApp.Models.Admin.Users
{
    public class UserCreateViewModel : IMapFrom<MyApp.DomainClasses.Users.User>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

    }
}