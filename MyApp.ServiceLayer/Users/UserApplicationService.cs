using AutoMapper;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Application.Services;
using MyApp.Framework.Data;
using MyApp.Models.Admin.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.ServiceLayer.Users
{
    public class UserApplicationService : CrudApplicationService<User, UserViewModel, UserCreateViewModel, UserEditViewModel, UserDeleteViewModel, UserPagedListRequest, UserListViewModel>,
        IUserApplicationService
    {
        public UserApplicationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public UserViewModel FindByUserName(string username)
        {
            return Mapper.Map<UserViewModel>(EntitySet.First(o => o.UserName == username));
        }
    }
}
