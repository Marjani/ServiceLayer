using AutoMapper;
using MyApp.DomainClasses.Roles;
using MyApp.Framework.Application.Services;
using MyApp.Framework.Data;
using MyApp.Models.Admin.Roles;

namespace MyApp.ServiceLayer.Roles
{
    public class RoleApplicationService :
        CrudApplicationService<Role, RoleViewModel, RoleCreateViewModel, RoleEditViewModel, RoleDeleteViewModel, RolePagedListRequest, RoleListViewModel>,
        IRoleApplicationService
    {
        #region Constructor

        public RoleApplicationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        #endregion
    }
}