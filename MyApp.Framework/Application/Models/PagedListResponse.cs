using System.Collections.Generic;

namespace MyApp.Framework.Application.Models
{
    public class PagedListResponse<TModel, TListRequest>
        where TListRequest : PagedListRequest, new()
        where TModel : IModel
    {
        public PagedListResponse()
        {
            Result = new List<TModel>();
            Request = new TListRequest();
        }
        public IList<TModel> Result { get; set; }
        public TListRequest Request { get; set; }
    }
}