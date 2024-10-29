using EBC.Core.Entities.Common;
using EBC.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccountManagerSystem.Repositories.Abstract;

public interface IGenericRepository<TEntity> : IGenericRepositoryWithoutBase<TEntity> 
    where TEntity : class
{
    // Soft Delete Methods
    int SoftDelete(Guid id);
    int SoftDelete(TEntity entity);
    Task<int> SoftDeleteAsync(Guid id);
    Task<int> SoftDeleteAsync(TEntity entity);




    // AddOrUpdate Method
    int AddOrUpdate(TEntity entity);
    Task<int> AddOrUpdateAsync(TEntity entity);




    // Queryable Methods
    IQueryable<TEntity> AsQueryable();
    IQueryable<TEntity> GetAllQueryable(bool noTracking = true);
    IQueryable<TEntity> GetAllQueryable(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);




    // Get Methods
    Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);




    // Utility Methods
    bool EntityAny();




    // Bulk Operations
    Task BulkAdd(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task BulkUpdate(IEnumerable<TEntity> entities);
    Task BulkDeleteById(IEnumerable<Guid> ids);
    Task BulkDelete(IEnumerable<TEntity> entities);
    Task BulkDelete(Expression<Func<TEntity, bool>> predicate);
}

