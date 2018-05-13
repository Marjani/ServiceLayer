using MyApp.Framework.Application.Models;
using MyApp.Models.Admin.Users;

namespace MyApp.Models.Admin.Users
{
    public class UserListViewModel : PagedListResponse<UserViewModel, UserPagedListRequest>
    {
        //other properties
    }
}
