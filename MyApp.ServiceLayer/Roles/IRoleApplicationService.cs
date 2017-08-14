using MyApp.Framework.Application.Services;
using MyApp.Models.Admin.Roles;

namespace MyApp.ServiceLayer.Roles
{
    public interface IRoleApplicationService :
        ICrudApplicationService<RoleViewModel, RoleCreateViewModel, RoleEditViewModel, RoleDeleteViewModel, RolePagedListRequest, RoleListViewModel>
    {
    }
}