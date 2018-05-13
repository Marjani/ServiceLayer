using MyApp.Framework.Application.Models;

namespace MyApp.Models.Admin.Users
{
    public class UserPagedListRequest : PagedListRequest
    {
        public string UserName { get; set; }
    }
}