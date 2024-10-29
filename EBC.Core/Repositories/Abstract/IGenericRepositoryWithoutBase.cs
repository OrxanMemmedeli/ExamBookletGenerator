using System.Linq.Expressions;

namespace EBC.Core.Repositories.Abstract;

public interface IGenericRepositoryWithoutBase<TEntity>
{
    // Add Methods
    int Add(TEntity entity);
    Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    int AddRange(IEnumerable<TEntity> entities);
    Task<int> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
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

    //Get Methods
    Task<List<TEntity>> GetAll(bool noTracking = true);


    // Save Changes
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
