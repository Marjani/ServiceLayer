using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFSecondLevelCache;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.DynamicFilters;
using MyApp.Framework.Data.Hooks;
using MyApp.Framework.Domain.Entities;
using MyApp.Framework.Domain.Entities.Tracking;
using MyApp.Framework.EntityFrameworkToolkit.Extensions;
using MyApp.Framework.GuardToolkit;
using MyApp.Framework.Infrastructure;

namespace MyApp.Framework.Data
{
    public abstract class DbContextBase : DbContext, IUnitOfWork
    {
        #region Constants

        private const string ConnectionStringName = "DefaultConnection";

        #endregion

        #region Constructors

        protected DbContextBase()
            : base(ConnectionStringName)
        {
            Configuration.LazyLoadingEnabled = false;
            HooksEnabled = true;
        }

        #endregion

        #region Private Methods

        private static void InvalidateSecondLevelCache(string[] changedEntityNames)
        {
            new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
        }

        #endregion

        #region NestedTypes

        private class DbContextTransactionAdapter : ITransaction
        {
            private readonly DbContextTransaction _transaction;

            public DbContextTransactionAdapter(DbContextTransaction transaction)
            {
                Guard.NotNull(transaction, nameof(transaction));

                _transaction = transaction;
            }

            public void Commit()
            {
                _transaction.Commit();
            }

            public void Rollback()
            {
                _transaction.Rollback();
            }

            public void Dispose()
            {
                _transaction.Dispose();
            }
        }

        private class HookEngine
        {
            private readonly DbContext _context;
            private readonly IList<HookedEntityEntry> _modifiedEntries;

            public HookEngine(DbContext context)
            {
                _context = context;
                _modifiedEntries = GetModifiedEntries();
            }

            public void ExecutePostActionHooks()
            {
                foreach (var entityEntry in _modifiedEntries)
                {
                    var entry = entityEntry;

                    var postActionHooks = IoC.GetAllInstances<IPostActionHook>()
                        .Where(x => (x.HookState & entry.PreSaveState) == entry.PreSaveState);
                    foreach (var hook in postActionHooks)
                    {
                        var metadata = new HookEntityMetadata(entityEntry.PreSaveState);
                        hook.Hook(entityEntry.Entity, metadata);
                    }
                }
            }

            public void ExecutePreActionHooks()
            {
                ExecutePreActionHooks(false);
                var hasValidationErrors =
                    _context.Configuration.ValidateOnSaveEnabled && _context.ChangeTracker.Entries()
                        .Any(x => x.State != EntityState.Unchanged &&
                                  !x.GetValidationResult().IsValid);
                if (!hasValidationErrors)
                    ExecutePreActionHooks(true);
            }

            private void ExecutePreActionHooks(bool requiresValidation)
            {
                foreach (var entityEntry in _modifiedEntries)
                {
                    var entry = entityEntry; //Prevents access to modified closure

                    var preActionHooks = IoC.GetAllInstances<IPreActionHook>()
                        .Where(x => (x.HookState & entry.PreSaveState) == entry.PreSaveState &&
                                    x.RequiresValidation == requiresValidation);

                    foreach (var hook in preActionHooks)
                    {
                        var metadata = new HookEntityMetadata(entityEntry.PreSaveState);

                        hook.Hook(entityEntry.Entity, metadata);

                        if (metadata.HasStateChanged)
                            entityEntry.PreSaveState = metadata.State;
                    }
                }
            }

            private List<HookedEntityEntry> GetModifiedEntries()
            {
                return _context.ChangeTracker.Entries<Entity>()
                    .Where(x => x.State != EntityState.Unchanged && x.State != EntityState.Detached)
                    .Select(x => new HookedEntityEntry
                    {
                        Entity = x.Entity,
                        PreSaveState = x.State
                    })
                    .ToList();
            }
        }

        #endregion

        #region IUnitOfWork Members

        #region Methods

        IDbSet<TEntity> IUnitOfWork.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public void MarkAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Added;
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public IList<T> SqlQuery<T>(string sql, params object[] parameters) where T : class
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public void ExecuteSqlCommand(string query)
        {
            Database.ExecuteSqlCommand(query);
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            Database.ExecuteSqlCommand(query, parameters);
        }

        public Task ExecuteSqlCommandAsync(string query)
        {
            return Database.ExecuteSqlCommandAsync(query);
        }

        public Task ExecuteSqlCommandAsync(string query, params object[] parameters)
        {
            return Database.ExecuteSqlCommandAsync(query, parameters);
        }

        public void BulkInsert<T>(IEnumerable<T> data)
        {
            BulkInsertExtension.BulkInsert(this, data);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                AutoDetectChangesEnabled = false;

                Set<TEntity>().AddRange(entities);
            }
            finally
            {
                AutoDetectChangesEnabled = true;
            }
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                AutoDetectChangesEnabled = false;

                Set<TEntity>().RemoveRange(entities);
            }
            finally
            {
                AutoDetectChangesEnabled = true;
            }
        }

        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                AutoDetectChangesEnabled = false;

                foreach (var entity in entities)
                {
                    MarkAsChanged(entity);
                }
            }
            finally
            {
                AutoDetectChangesEnabled = true;
            }
        }

        public void ForceDatabaseInitialize()
        {
            Database.Initialize(true);
        }

        public override int SaveChanges()
        {
            return SaveChanges(false);
        }

        public int SaveChanges(bool invalidateSecondLevelCache)
        {
            ChangeTracker.DetectChanges();

            AutoDetectChangesEnabled = false;

            var hookEngine = new HookEngine(this);
            hookEngine.ExecutePreActionHooks();

            var changedEntityNames = ChangeTracker.GetChangedEntityNames();

            var result = base.SaveChanges();

            hookEngine.ExecutePostActionHooks();

            AutoDetectChangesEnabled = true;

            if (!invalidateSecondLevelCache) return result;

            InvalidateSecondLevelCache(changedEntityNames);

            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return SaveChangesAsync(false, cancellationToken);
        }

        public Task<int> SaveChangesAsync(bool invalidateSecondLevelCache, CancellationToken cancellationToken)
        {
            ChangeTracker.DetectChanges();

            AutoDetectChangesEnabled = false;

            var hookEngine = new HookEngine(this);
            hookEngine.ExecutePreActionHooks();

            var changedEntityNames = ChangeTracker.GetChangedEntityNames();

            var result = base.SaveChangesAsync(cancellationToken);

            hookEngine.ExecutePostActionHooks();

            AutoDetectChangesEnabled = true;

            if (!invalidateSecondLevelCache) return result;

            InvalidateSecondLevelCache(changedEntityNames);

            return result;
        }

        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public Task<int> SaveChangesAsync(bool invalidateSecondLevelCache)
        {
            return SaveChangesAsync(invalidateSecondLevelCache, CancellationToken.None);
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            return new DbContextTransactionAdapter(Database.BeginTransaction(isolationLevel));
        }

        public void DisableFilter(string filterName)
        {
            DynamicFilterExtensions.DisableFilter(this, filterName);
        }

        public void EnableRowLevelSecurity(Func<long?> userIdFunc)
        {
            var userId = userIdFunc?.Invoke();

            if (!userId.HasValue)
                throw new InvalidOperationException(
                    $"for enable Row Level Security {nameof(userId)} must be has value (user must be authenticated)");

            EnableFilter(nameof(IHasRowLevelSecurity));
            this.SetFilterScopedParameterValue(nameof(IHasRowLevelSecurity), nameof(userId), userId.Value);
        }

        public void EnableFilter(string filterName)
        {
            DynamicFilterExtensions.EnableFilter(this, filterName);
        }

        public void EnableFilter(string filterName, object parameterValue)
        {
            EnableFilter(filterName);
            this.SetFilterScopedParameterValue(filterName, parameterValue);
        }

        public void ChangeFilterParameterValue(string filterName, object parameterValue)
        {
            this.SetFilterScopedParameterValue(filterName, parameterValue);
        }

        public void DisableAllFilters()
        {
            DynamicFilterExtensions.DisableAllFilters(this);
        }

        public void EnableAllFilters()
        {
            DynamicFilterExtensions.EnableAllFilters(this);
        }

        #endregion

        #region Properties

        public bool AutoDetectChangesEnabled
        {
            get { return Configuration.AutoDetectChangesEnabled; }
            set { Configuration.AutoDetectChangesEnabled = value; }
        }

        public bool ValidateOnSaveEnabled
        {
            get { return Configuration.ValidateOnSaveEnabled; }
            set { Configuration.ValidateOnSaveEnabled = value; }
        }

        public bool UseDatabaseNullSemantics
        {
            get { return Configuration.UseDatabaseNullSemantics; }
            set { Configuration.UseDatabaseNullSemantics = value; }
        }

        public bool HooksEnabled { get; set; }

        #endregion

        #endregion

        #region Protected Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Guard.ArgumentNotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.AddFrameworkConventions();
            modelBuilder.ConfigureFrameworkFilters();

            modelBuilder.Ignore<Entity>();
            modelBuilder.Ignore<CreationTrackingEntity>();
            modelBuilder.Ignore<ModificationTrackingEntity>();
            modelBuilder.Ignore<DeletionTrackingEntity>();
            modelBuilder.Ignore<TrackableEntity>();
            modelBuilder.Ignore<FullTrackableEntity>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion
    }
}