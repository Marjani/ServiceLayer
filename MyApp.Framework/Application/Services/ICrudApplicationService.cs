using System.Collections.Generic;
using System.Threading.Tasks;
using MyApp.Framework.Application.Models;

namespace MyApp.Framework.Application.Services
{
    public interface ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel> :
        ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel, PagedListRequest,
            PagedListResponse<TModel, PagedListRequest>, DynamicListRequest>
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
    {
    }

    public interface ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel, in TDynamicListRequest> :
        ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel, PagedListRequest,
            PagedListResponse<TModel, PagedListRequest>, TDynamicListRequest>
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
        where TDynamicListRequest : DynamicListRequest
    {
    }

    public interface ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel, in TPagedListRequest,
        TPagedListResponse> :
        ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel, TPagedListRequest, TPagedListResponse,
            DynamicListRequest>
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
        where TPagedListRequest : PagedListRequest, new()
        where TPagedListResponse : PagedListResponse<TModel, TPagedListRequest>
    {
    }

    public interface ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel, in TPagedListRequest,
        TPagedListResponse,
        in TDynamicListRequest> : IApplicationService
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
        where TPagedListRequest : PagedListRequest, new()
        where TPagedListResponse : PagedListResponse<TModel, TPagedListRequest>
        where TDynamicListRequest : DynamicListRequest
    {
        void Create(TCreateModel model);
        void Create(IList<TCreateModel> models);
        Task CreateAsync(TCreateModel model);
        Task CreateAsync(IList<TCreateModel> models);

        IList<TModel> GetList();
        DynamicListResponse GetDynamicList(TDynamicListRequest request);
        TPagedListResponse GetPagedList(TPagedListRequest request);
        IList<LookupItem> GetLookup();
        TModel GetById(long id);
        TEditModel GetForEdit(long id);
        bool Exists(long id);
        Task<IList<TModel>> GetListAsync();
        Task<DynamicListResponse> GetDynamicListAsync(TDynamicListRequest request);
        Task<TPagedListResponse> GetPagedListAsync(TPagedListRequest request);
        Task<IList<LookupItem>> GetLookupAsync();
        Task<TModel> GetByIdAsync(long id);
        Task<TEditModel> GetForEditAsync(long id);
        Task<bool> ExistsAsync(long id);

        void Edit(TEditModel model);
        void Edit(IList<TEditModel> models);
        Task EditAsync(TEditModel model);
        Task EditAsync(IList<TEditModel> models);
        
        void Delete(TDeleteModel model);
        void Delete(IList<TDeleteModel> models);
        Task DeleteAsync(TDeleteModel model);
        Task DeleteAsync(IList<TDeleteModel> models);
    }
}