using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MyApp.Framework.Application.Models;
using MyApp.Framework.Aspects.Transaction;
using MyApp.Framework.Data;
using MyApp.Framework.Domain.Entities;
using MyApp.Framework.EntityFrameworkToolkit.Extensions;
using MyApp.Framework.Extensions;
using MyApp.Framework.GuardToolkit;

namespace MyApp.Framework.Application.Services
{
    public abstract class CrudApplicationService<TEntity, TModel, TCreateModel, TEditModel, TDeleteModel> :
        CrudApplicationService<TEntity, TModel, TCreateModel, TEditModel, TDeleteModel, PagedListRequest,
            PagedListResponse<TModel, PagedListRequest>, DynamicListRequest>
        where TEntity : Entity
        where TCreateModel : class
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
    {
        protected CrudApplicationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }

    public abstract class CrudApplicationService<TEntity, TModel, TCreateModel, TEditModel, TDeleteModel,
        TDynamicListRequest> :
        CrudApplicationService<TEntity, TModel, TCreateModel, TEditModel, TDeleteModel, PagedListRequest,
            PagedListResponse<TModel, PagedListRequest>, TDynamicListRequest>
        where TEntity : Entity
        where TCreateModel : class
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
        where TDynamicListRequest : DynamicListRequest
    {
        protected CrudApplicationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }

    public abstract class CrudApplicationService<TEntity, TModel, TCreateModel, TEditModel, TDeleteModel,
        TPagedListRequest,
        TPagedListResponse> :
        CrudApplicationService<TEntity, TModel, TCreateModel, TEditModel, TDeleteModel, TPagedListRequest,
            TPagedListResponse,
            DynamicListRequest>
        where TEntity : Entity
        where TCreateModel : class
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
        where TPagedListRequest : PagedListRequest, new()
        where TPagedListResponse : PagedListResponse<TModel, TPagedListRequest>, new()
    {
        protected CrudApplicationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }

    public abstract class CrudApplicationService<TEntity, TModel, TCreateModel, TEditModel, TDeleteModel,
        TPagedListRequest,
        TPagedListResponse, TDynamicListRequest> : ApplicationService,
        ICrudApplicationService<TModel, TCreateModel, TEditModel, TDeleteModel, TPagedListRequest, TPagedListResponse,
            TDynamicListRequest>
        where TEntity : Entity
        where TCreateModel : class
        where TEditModel : class, IEditModel
        where TModel : class, IModel
        where TDeleteModel : class, IDeleteModel
        where TPagedListRequest : PagedListRequest, new()
        where TPagedListResponse : PagedListResponse<TModel, TPagedListRequest>, new()
        where TDynamicListRequest : DynamicListRequest

    {
        #region Constructor

        protected CrudApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            Guard.ArgumentNotNull(unitOfWork, nameof(unitOfWork));
            Guard.ArgumentNotNull(mapper, nameof(mapper));

            UnitOfWork = unitOfWork;
            Mapper = mapper;
            EntitySet = UnitOfWork.Set<TEntity>();
        }

        #endregion

        #region Properties

        protected IQueryable<TEntity> UnTrackedEntitySet => EntitySet.AsNoTracking();
        protected IUnitOfWork UnitOfWork { get; }
        protected IMapper Mapper { get; }
        protected IDbSet<TEntity> EntitySet { get; }

        #endregion

        #region ICrudApplicationService Members

        #region Methods

        [Transactional]
        public virtual void Create(TCreateModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            var entity = Mapper.Map<TEntity>(model);

            EntitySet.Add(entity);
            UnitOfWork.SaveChanges();
        }

        [Transactional]
        public virtual void Create(IList<TCreateModel> models)
        {
            Guard.ArgumentNotEmpty(models, nameof(models));

            var entities = Mapper.Map<IList<TEntity>>(models);

            UnitOfWork.AddRange(entities);
            UnitOfWork.SaveChanges();
        }

        [Transactional]
        public virtual Task CreateAsync(TCreateModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            var entity = Mapper.Map<TEntity>(model);

            EntitySet.Add(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        [Transactional]
        public virtual Task CreateAsync(IList<TCreateModel> models)
        {
            Guard.ArgumentNotEmpty(models, nameof(models));

            var entities = Mapper.Map<IList<TEntity>>(models);

            UnitOfWork.AddRange(entities);
            return UnitOfWork.SaveChangesAsync();
        }


        [Transactional]
        public virtual void Edit(TEditModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            var entity = Mapper.Map<TEntity>(model);

            UnitOfWork.MarkAsChanged(entity);
            UnitOfWork.SaveChanges();
        }

        [Transactional]
        public virtual void Edit(IList<TEditModel> models)
        {
            Guard.ArgumentNotNull(models, nameof(models));
            Guard.ArgumentNotEmpty(models, nameof(models));

            var entities = Mapper.Map<IList<TEntity>>(models);

            UnitOfWork.UpdateRange(entities);
            UnitOfWork.SaveChanges();
        }

        [Transactional]
        public virtual Task EditAsync(TEditModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            var entity = Mapper.Map<TEntity>(model);

            UnitOfWork.MarkAsChanged(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        [Transactional]
        public virtual Task EditAsync(IList<TEditModel> models)
        {
            Guard.ArgumentNotNull(models, nameof(models));
            Guard.ArgumentNotEmpty(models, nameof(models));

            var entities = Mapper.Map<IList<TEntity>>(models);

            UnitOfWork.UpdateRange(entities);
            return UnitOfWork.SaveChangesAsync();
        }


        public virtual IList<TModel> GetList()
        {
            return EntitySet.ProjectToList<TModel>(Mapper.ConfigurationProvider);
        }

        public virtual DynamicListResponse GetDynamicList(TDynamicListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            var query = ApplyFiltering(request);

            return query.ProjectTo<TModel>().ToListResponse(request);
        }

        public virtual TPagedListResponse GetPagedList(TPagedListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            var query = ApplyFiltering(request);

            request.TotalCount = query.LongCount();

            query = ApplySorting(query, request);
            query = ApplyPaging(query, request);

            request.PageNumber++;

            var result = query.ProjectToList<TModel>(Mapper.ConfigurationProvider);

            return new TPagedListResponse
            {
                Result = result,
                Request = request
            };
        }

        public virtual IList<LookupItem> GetLookup()
        {
            return EntitySet.ProjectToList<LookupItem>(Mapper.ConfigurationProvider);
        }

        public virtual TModel GetById(long id)
        {
            Guard.ArgumentInRange(id, 1, long.MaxValue, nameof(id));

            var entity =
                EntitySet.Where(a => a.Id == id).ProjectToFirstOrDefault<TModel>(Mapper.ConfigurationProvider);

            if (entity == null)
                throw new EntityNotFoundException($"Couldn't Find Entity {id} When GetById");

            return entity;
        }

        public virtual TEditModel GetForEdit(long id)
        {
            Guard.ArgumentInRange(id, 1, long.MaxValue, nameof(id));

            var entity =
                EntitySet.Where(a => a.Id == id).ProjectToFirstOrDefault<TEditModel>(Mapper.ConfigurationProvider);

            if (entity == null)
                throw new EntityNotFoundException($"Couldn't Find Entity {id} When GetForEdit");

            return entity;
        }

        public virtual bool Exists(long id)
        {
            Guard.ArgumentInRange(id, 1, long.MaxValue, nameof(id));

            return EntitySet.Any(a => a.Id == id);
        }

        public virtual async Task<IList<TModel>> GetListAsync()
        {
            return await EntitySet.ProjectToListAsync<TModel>(Mapper.ConfigurationProvider);
        }

        public virtual Task<DynamicListResponse> GetDynamicListAsync(TDynamicListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            var query = ApplyFiltering(request);

            return query.ProjectTo<TModel>().ToListResponseAsync(request);
        }

        public virtual async Task<TPagedListResponse> GetPagedListAsync(TPagedListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            var query = ApplyFiltering(request);

            request.TotalCount = await query.LongCountAsync().ConfigureAwait(false);

            query = ApplySorting(query, request);
            query = ApplyPaging(query, request);

            request.PageNumber++;

            var result = await query.ProjectToListAsync<TModel>(Mapper.ConfigurationProvider).ConfigureAwait(false);

            return new TPagedListResponse
            {
                Result = result,
                Request = request
            };
        }

        public virtual async Task<IList<LookupItem>> GetLookupAsync()
        {
            return await EntitySet.ProjectToListAsync<LookupItem>(Mapper.ConfigurationProvider);
        }

        public virtual async Task<TModel> GetByIdAsync(long id)
        {
            Guard.ArgumentInRange(id, 1, long.MaxValue, nameof(id));

            var entity = await UnTrackedEntitySet.Where(a => a.Id == id)
                .ProjectToFirstOrDefaultAsync<TModel>(Mapper.ConfigurationProvider);

            if (entity == null)
                throw new EntityNotFoundException($"Couldn't Find Entity {id} When GetByIdAsync");

            return entity;
        }

        public virtual async Task<TEditModel> GetForEditAsync(long id)
        {
            Guard.ArgumentInRange(id, 1, long.MaxValue, nameof(id));

            var entity = await UnTrackedEntitySet.Where(a => a.Id == id)
                .ProjectToFirstOrDefaultAsync<TEditModel>(Mapper.ConfigurationProvider);

            if (entity == null)
                throw new EntityNotFoundException($"Couldn't Find Entity {id} When GetForEditAsync");

            return entity;
        }

        public virtual Task<bool> ExistsAsync(long id)
        {
            Guard.ArgumentInRange(id, 1, long.MaxValue, nameof(id));

            return EntitySet.AnyAsync(a => a.Id == id);
        }


        [Transactional]
        public virtual void Delete(TDeleteModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            var entity = Mapper.Map<TEntity>(model);

            UnitOfWork.MarkAsDeleted(entity);
            UnitOfWork.SaveChanges();
        }

        [Transactional]
        public virtual void Delete(IList<TDeleteModel> models)
        {
            Guard.ArgumentNotEmpty(models, nameof(models));
            Guard.ArgumentNotEmpty(models, nameof(models));

            var entities = Mapper.Map<IList<TEntity>>(models);

            UnitOfWork.RemoveRange(entities);
            UnitOfWork.SaveChanges();
        }

        [Transactional]
        public virtual Task DeleteAsync(TDeleteModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));

            var entity = Mapper.Map<TEntity>(model);

            UnitOfWork.MarkAsDeleted(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        [Transactional]
        public virtual Task DeleteAsync(IList<TDeleteModel> models)
        {
            Guard.ArgumentNotEmpty(models, nameof(models));
            Guard.ArgumentNotEmpty(models, nameof(models));

            var entities = Mapper.Map<IList<TEntity>>(models);

            UnitOfWork.RemoveRange(entities);
            return UnitOfWork.SaveChangesAsync();
        }

        #endregion

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Apply Filtering To GetDynamicList
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ApplyFiltering(TDynamicListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            return UnTrackedEntitySet;
        }

        /// <summary>
        ///     Apply Filtering To GetPagedList and GetPagedListAsync
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ApplyFiltering(TPagedListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            return UnTrackedEntitySet;
        }

        /// <summary>
        ///     Apply Sorting To GetPagedList and GetPagedListAsync
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="request">PagedListRequest</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TPagedListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));
            Guard.ArgumentNotNull(query, nameof(query));

            return !request.SortBy.IsEmpty() ? query.OrderBy(request.SortBy) : query.OrderByDescending(e => e.Id);
        }

        /// <summary>
        ///     Apply Paging To GetPagedList and GetPagedListAsync
        /// </summary>
        /// <param name="request">PagedListRequest</param>
        /// <param name="query">query</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TPagedListRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));
            Guard.ArgumentNotNull(query, nameof(query));

            return request != null
                ? query.Page((request.PageNumber - 1) * request.PageSize, request.PageSize)
                : query;
        }

        #endregion
    }
}