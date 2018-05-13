using MyApp.Framework.Application.Services;
using MyApp.Models.Admin.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.ServiceLayer.Users
{
    public interface IUserApplicationService:
         ICrudApplicationService<UserViewModel, UserCreateViewModel, UserEditViewModel, UserDeleteViewModel, UserPagedListRequest, UserListViewModel>
    {
        UserViewModel FindByUserName(string username);

    }
}
