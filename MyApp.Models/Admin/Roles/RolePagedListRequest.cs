using MyApp.Framework.Application.Models;

namespace MyApp.Models.Admin.Roles
{
    public class RolePagedListRequest : PagedListRequest
    {
        public string RoleName { get; set; }
    }
}