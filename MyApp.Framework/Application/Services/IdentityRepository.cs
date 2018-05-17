using AutoMapper;
using MyApp.Framework.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using MyApp.Framework.Domain.Entities;

namespace MyApp.Framework.Application.Services
{
    public class IdentityRepository<TEntity> : IIdentityRepository<TEntity> where TEntity : class
    {
        public IdentityRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {

            UnitOfWork = unitOfWork;
            Mapper = mapper;
            EntitySet = UnitOfWork.Set<TEntity>();
        }



        protected IQueryable<TEntity> UnTrackedEntitySet => EntitySet.AsNoTracking();
        protected IUnitOfWork UnitOfWork { get; }
        protected IMapper Mapper { get; }
        protected IDbSet<TEntity> EntitySet { get; }


        public void Add(TEntity entity)
        {
            UnitOfWork.AddRange<TEntity>(new TEntity[] { entity });
            UnitOfWork.SaveChanges();
        }

        public TEntity FindById(object id)
        {
            var entity =
                 EntitySet.Find(id);

            if (entity == null)
                throw new EntityNotFoundException($"Couldn't Find Entity {id} When GetById");

            return entity;
        }

        public Task<TEntity> FindByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> GetAll()
        {
            return EntitySet.ToList();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return EntitySet.ToListAsync();
        }

        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return EntitySet.ToListAsync(cancellationToken);
        }

        public List<TEntity> PageAll(int skip, int take)
        {
return EntitySet.Skip(skip).Take(take).ToList();
        }

        public Task<List<TEntity>> PageAllAsync(int skip, int take)
        {
            return EntitySet.Skip(skip).Take(take).ToListAsync();
        }

        public Task<List<TEntity>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return EntitySet.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public void Remove(TEntity entity)
        {
            EntitySet.Remove(entity);        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();

        }
    }
}
