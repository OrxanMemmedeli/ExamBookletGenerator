using EBC.Core.IEntities.Common;
using System.Linq.Expressions;

namespace AccountManagerSystem.Repositories.Abstract;

public interface IGenericRepository<TEntity> where TEntity : IBaseEntity
{

    // Add Methods
    int Add(TEntity entity);
    Task<int> AddAsync(TEntity entity);

    int AddRange(IEnumerable<TEntity> entities);
    Task<int> AddRangeAsync(IEnumerable<TEntity> entities);
    
    void AddWithoutSave(TEntity entity);
    void AddRangeWithoutSave(IEnumerable<TEntity> entities);
    Task<int> AddRangeAsyncWithoutSave(IEnumerable<TEntity> entities);




    // Update Methods
    int Update(TEntity entity);
    Task<int> UpdateAsync(TEntity entity);

    void UpdateWithoutSave(TEntity entity);




    // Delete Methods
    int Delete(Guid id);
    int Delete(TEntity entity);
    Task<int> DeleteAsync(Guid id);
    Task<int> DeleteAsync(TEntity entity);

    bool DeleteRange(Expression<Func<TEntity, bool>> predicate);
    Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);

    void DeleteRangeWithoutSave(IEnumerable<TEntity> entities);
    Task DeleteRangeAsyncWithoutSave(IEnumerable<TEntity> entities);




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
    Task<List<TEntity>> GetAll(bool noTracking = true);
    Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes);




    // Utility Methods
    bool EntityAny();




    // Bulk Operations
    Task BulkAdd(IEnumerable<TEntity> entities);
    Task BulkUpdate(IEnumerable<TEntity> entities);
    Task BulkDeleteById(IEnumerable<Guid> ids);
    Task BulkDelete(IEnumerable<TEntity> entities);
    Task BulkDelete(Expression<Func<TEntity, bool>> predicate);




    // Save Changes
    Task<int> SaveChangesAsync();
    int SaveChanges();
}
