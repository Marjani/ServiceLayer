using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Framework.Data
{
    public interface IUnitOfWork : IDisposable
    {
        bool AutoDetectChangesEnabled { get; set; }
        bool ValidateOnSaveEnabled { get; set; }
        bool UseDatabaseNullSemantics { get; set; }
        bool HooksEnabled { get; set; }
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        void MarkAsAdded<TEntity>(TEntity entity) where TEntity : class;
        void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class;
        void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class;

        IList<T> SqlQuery<T>(string sql, params object[] parameters) where T : class;
        void ExecuteSqlCommand(string query);
        void ExecuteSqlCommand(string query, params object[] parameters);
        Task ExecuteSqlCommandAsync(string query);
        Task ExecuteSqlCommandAsync(string query, params object[] parameters);

        void BulkInsert<T>(IEnumerable<T> data);
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        void ForceDatabaseInitialize();

        int SaveChanges();
        int SaveChanges(bool invalidateSecondLevelCache);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool invalidateSecondLevelCache, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(bool invalidateSecondLevelCache);

        ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot);

        void DisableFilter(string filterName);
        void EnableRowLevelSecurity(Func<long?> userIdFunc);
        void EnableFilter(string filterName);
        void EnableFilter(string filterName, object parameterValue);
        void ChangeFilterParameterValue(string filterName, object parameterValue);
        void DisableAllFilters();
        void EnableAllFilters();
    }
}