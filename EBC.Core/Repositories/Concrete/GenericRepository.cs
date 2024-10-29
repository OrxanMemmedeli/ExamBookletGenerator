using System;
using System.Linq.Expressions;
using System.Threading;
using AccountManagerSystem.Repositories.Abstract;
using EBC.Core.Entities.Common;
using EBC.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Repositories.Concrete;


public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IGenericRepositoryWithoutBase<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;

    public GenericRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected DbSet<TEntity> entity => _context.Set<TEntity>();


    #region Add Methods

    public virtual int Add(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        this.entity.Add(entity);
        return SaveChanges();
    }

    public virtual async Task<int> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        await this.entity.AddAsync(entity, cancellationToken);
        return await SaveChangesAsync(cancellationToken);
    }

    public virtual int AddRange(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return 0;

        this.entity.AddRange(entities);
        return SaveChanges();
    }

    public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null || !entities.Any())
            return 0;

        await this.entity.AddRangeAsync(entities, cancellationToken);
        return await SaveChangesAsync(cancellationToken);
    }

    public virtual void AddWithoutSave(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        this.entity.Add(entity);
    }

    public virtual void AddRangeWithoutSave(IEnumerable<TEntity> entities)
    {
        if (entities != null && entities.Any())
            this.entity.AddRange(entities);
    }

    public virtual async Task<int> AddRangeAsyncWithoutSave(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return 0;

        await this.entity.AddRangeAsync(entities);
        return 1; // Əlavə edilənlərin sayını qaytarmırıq, yalnız əməliyyatın uğurlu olduğunu bildiririk.
    }

    #endregion



    #region Update Methods

    public virtual int Update(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        this.entity.Update(entity);
        return SaveChanges();
    }

    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        _context.Entry(entity).State = EntityState.Modified;
        return await SaveChangesAsync();
    }

    public virtual void UpdateWithoutSave(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        _context.Entry(entity).State = EntityState.Modified;
    }

    #endregion



    #region Delete Methods

    public virtual int Delete(Guid id)
    {
        var entity = this.entity.Find(id);
        if (entity == null) return 0;
        return Delete(entity);
    }

    public virtual int Delete(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (_context.Entry(entity).State == EntityState.Detached)
            this.entity.Attach(entity);

        this.entity.Remove(entity);
        return SaveChanges();
    }

    public virtual async Task<int> DeleteAsync(Guid id)
    {
        var entity = await this.entity.FindAsync(id);
        if (entity == null) return 0;
        return await DeleteAsync(entity);
    }

    public virtual async Task<int> DeleteAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (_context.Entry(entity).State == EntityState.Detached)
            this.entity.Attach(entity);

        this.entity.Remove(entity);
        return await SaveChangesAsync();
    }

    public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = entity.Where(predicate).ToList();
        if (!entities.Any()) return false;

        _context.RemoveRange(entities);
        return SaveChanges() > 0;
    }

    public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await entity.Where(predicate).ToListAsync();
        if (!entities.Any()) return false;

        _context.RemoveRange(entities);
        int affectedRows = await SaveChangesAsync();
        return affectedRows > 0;
    }

    public virtual void DeleteRangeWithoutSave(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any()) return;
        _context.RemoveRange(entities);
    }

    public virtual async Task DeleteRangeAsyncWithoutSave(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any()) return;
        _context.RemoveRange(entities);
        await Task.CompletedTask; // Boş bir asinxron tapşırıq döndərir
    }

    #endregion



    #region Soft Delete Methods

    public virtual int SoftDelete(Guid id)
    {
        var entity = this.entity.Find(id);
        if (entity == null) return 0;

        return SoftDeleteEntity(entity);
    }

    public virtual async Task<int> SoftDeleteAsync(Guid id)
    {
        var entity = await this.entity.FindAsync(id);
        if (entity == null) return 0;

        return await SoftDeleteEntityAsync(entity);
    }

    public virtual int SoftDelete(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));

        return SoftDeleteEntity(entity);
    }

    public virtual async Task<int> SoftDeleteAsync(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));

        return await SoftDeleteEntityAsync(entity);
    }

    private int SoftDeleteEntity(TEntity entity)
    {
        if (entity is BaseEntity<Guid> baseEntity)
        {
            baseEntity.IsDeleted = true;
            return SaveChanges();
        }
        throw new InvalidOperationException("Soft delete is only supported for entities implementing BaseEntity<Guid>.");
    }

    private async Task<int> SoftDeleteEntityAsync(TEntity entity)
    {
        if (entity is BaseEntity<Guid> baseEntity)
        {
            baseEntity.IsDeleted = true;
            return await SaveChangesAsync();
        }
        throw new InvalidOperationException("Soft delete is only supported for entities implementing BaseEntity<Guid>.");
    }
    #endregion



    #region AddOrUpdate Methods

    /// <summary>
    /// Verilən entity-ni verilənlər bazasına əlavə edir və ya mövcud olduqda onu yeniləyir. 
    /// Əgər entity artıq verilənlər bazasında varsa, entity dəyişiklikləri saxlanılır, əks halda yeni olaraq əlavə edilir.
    /// </summary>
    /// <param name="entity">Əlavə və ya yenilənəcək entity.</param>
    /// <returns>Uğurlu əməliyyat nəticəsində təsir edilən sətirlərin sayı.</returns>
    public virtual int AddOrUpdate(TEntity entity)
        => AddOrUpdateInternal(entity, isAsync: false).Result;

    /// <summary>
    /// Verilən entity-ni verilənlər bazasına asinxron olaraq əlavə edir və ya mövcud olduqda onu yeniləyir. 
    /// Əgər entity artıq verilənlər bazasında varsa, entity dəyişiklikləri saxlanılır, əks halda yeni olaraq əlavə edilir.
    /// </summary>
    /// <param name="entity">Əlavə və ya yenilənəcək entity.</param>
    /// <returns>Uğurlu əməliyyat nəticəsində təsir edilən sətirlərin sayı.</returns>
    public virtual async Task<int> AddOrUpdateAsync(TEntity entity)
        => await AddOrUpdateInternal(entity, isAsync: true);


    private async Task<int> AddOrUpdateInternal(TEntity entity, bool isAsync = true)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        if (entity is not Entity<Guid> baseEntity)
            throw new InvalidOperationException($"AddOrUpdate is only supported for entities implementing Entity<Guid>.");

        // Entity verilənlər bazasında mövcuddursa, onu yeniləyir
        bool exists = this.entity.Local.Any(e => (e as Entity<Guid>)?.Id == baseEntity.Id) ||
                      (isAsync
                        ? await this.entity.AnyAsync(e => (e as Entity<Guid>).Id == baseEntity.Id)
                        : this.entity.Any(e => (e as Entity<Guid>).Id == baseEntity.Id));

        // Mövcud entity-nin vəziyyəti "Modified" olaraq təyin edilir/ Mövcud deyilsə, yeni entity olaraq əlavə edilir
        _context.Entry(entity).State = exists ? EntityState.Modified : EntityState.Added;
        return isAsync ? await SaveChangesAsync() : SaveChanges();
    }

    #endregion



    #region Queryable Methods

    public virtual IQueryable<TEntity> AsQueryable() => entity.AsQueryable();

    /// <summary>
    /// No-tracking rejimində ya da tracking ilə bütün entity-ləri queryable şəklində döndürür.
    /// </summary>
    /// <param name="noTracking">No-tracking rejimi (default olaraq true).</param>
    /// <returns>Queryable entity kolleksiyası.</returns>
    public virtual IQueryable<TEntity> GetAllQueryable(bool noTracking = true)
        => noTracking ? entity.AsNoTracking() : entity;

    /// <summary>
    /// Şərtə və əlavə parametrlərə əsasən, entity kolleksiyasını queryable şəklində döndürür.
    /// </summary>
    /// <param name="predicate">Filtr üçün predicate.</param>
    /// <param name="noTracking">No-tracking rejimi (default olaraq true).</param>
    /// <param name="orderBy">Sıralama funksiyası.</param>
    /// <param name="includes">Daxil ediləcək əlaqəli entity-lər.</param>
    /// <returns>Queryable entity kolleksiyası.</returns>
    public virtual IQueryable<TEntity> GetAllQueryable(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (predicate != null)
            query = query.Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        if (orderBy != null)
            query = orderBy(query);

        return noTracking ? query.AsNoTracking() : query;
    }


    /* Istifade nümunəsi
     
    var filteredEntities = genericRepository.GetAllQueryable(
        predicate: entity => entity.IsDeleted == false, // Şərt: Silinməyən entity-lər
        noTracking: true, // No-tracking rejimində
        orderBy: query => query.OrderByDescending(e => e.CreatedDate), // Sıralama: Yaranma tarixinə görə
        includes: e => e.RelatedEntity // Əlaqəli entity
    );
     
     */

    #endregion



    #region Get Methods

    /// <summary>
    /// Verilənlər bazasından bütün entity-ləri alır və siyahı şəklində döndürür.
    /// </summary>
    /// <param name="noTracking">No-tracking rejimi (default olaraq true).</param>
    /// <returns>Entity-lərdən ibarət siyahı.</returns>
    public virtual async Task<List<TEntity>> GetAll(bool noTracking = true)
    {
        return noTracking
            ? await entity.AsNoTracking().ToListAsync()
            : await entity.ToListAsync();
    }


    /// <summary>
    /// Filtr və digər parametrlərə əsasən entity kolleksiyasını alır və siyahı şəklində döndürür.
    /// </summary>
    /// <param name="predicate">Filtr üçün predicate.</param>
    /// <param name="noTracking">No-tracking rejimi (default olaraq true).</param>
    /// <param name="orderBy">Sıralama funksiyası.</param>
    /// <param name="includes">Daxil ediləcək əlaqəli entity-lər.</param>
    /// <returns>Filtrə uyğun gələn entity-lərdən ibarət siyahı.</returns>
    public virtual async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (predicate != null)
            query = query.Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        if (orderBy != null)
            query = orderBy(query);

        if (noTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }


    /// <summary>
    /// Verilmiş id-yə uyğun gələn entity-ni alır və döndürür.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <param name="noTracking">No-tracking rejimi (default olaraq true).</param>
    /// <param name="includes">Daxil ediləcək əlaqəli entity-lər.</param>
    /// <returns>Entity və ya null.</returns>
    public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object?>>[] includes)
    {
        var foundEntity = await entity.FindAsync(id);

        if (foundEntity == null)
            throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} with ID {id} not found.");

        if (noTracking)
            _context.Entry(foundEntity).State = EntityState.Detached;

        foreach (var include in includes)
            _context.Entry(foundEntity).Reference(include).Load();

        return foundEntity;
    }




    /// <summary>
    /// Verilmiş predicate ilə uyğun gələn ilk entity-ni alır və döndürür.
    /// </summary>
    /// <param name="predicate">Filtr üçün predicate.</param>
    /// <param name="noTracking">No-tracking rejimi (default olaraq true).</param>
    /// <param name="includes">Daxil ediləcək əlaqəli entity-lər.</param>
    /// <returns>Predicate uyğun gələn ilk entity və ya null.</returns>
    public virtual Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        => GetAllQueryable(predicate, noTracking, null, includes).FirstOrDefaultAsync();


    /// <summary>
    /// Verilmiş predicate ilə uyğun gələn tək entity-ni alır və döndürür. Bir neçə uyğun nəticə varsa, yalnız biri seçilir.
    /// </summary>
    /// <param name="predicate">Filtr üçün predicate.</param>
    /// <param name="noTracking">No-tracking rejimi (default olaraq true).</param>
    /// <param name="includes">Daxil ediləcək əlaqəli entity-lər.</param>
    /// <returns>Predicate uyğun gələn tək entity və ya null.</returns>
    public virtual async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (predicate != null)
            query = query.Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        if (noTracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync();
    }

    #endregion

    /// <summary>
    /// Verilənlər bazasında hər hansı bir qeyd olub-olmadığını yoxlayır.
    /// </summary>
    /// <returns>Əgər entity-lər varsa, true; əks halda false qaytarır.</returns>
    public bool EntityAny()
        => entity.AsNoTracking().Any();


    #region Bulk

    /// <summary>
    /// Verilən entity-ləri toplu şəkildə verilənlər bazasına əlavə edir.
    /// </summary>
    /// <param name="entities">Əlavə ediləcək entity-lərin siyahısı.</param>
    public virtual async Task BulkAdd(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null || !entities.Any())
            return; // Boş və ya null siyahı olduqda heç bir əməliyyat etmə.

        await entity.AddRangeAsync(entities, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Şərtə uyğun olan entity-ləri toplu şəkildə verilənlər bazasından silir.
    /// </summary>
    /// <param name="predicate">Silinmə üçün şərt ifadəsi.</param>
    public virtual Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
    {
        _context.RemoveRange(entity.Where(predicate));
        return SaveChangesAsync();
    }

    /// <summary>
    /// Verilən entity-ləri toplu şəkildə verilənlər bazasından silir.
    /// </summary>
    /// <param name="entities">Silinəcək entity-lərin siyahısı.</param>
    public virtual Task BulkDelete(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return Task.CompletedTask; // Boş və ya null siyahı olduqda heç bir əməliyyat etmə.

        entity.RemoveRange(entities);
        return SaveChangesAsync();
    }

    /// <summary>
    /// ID-ləri verilmiş entity-ləri toplu şəkildə verilənlər bazasından silir.
    /// </summary>
    /// <param name="ids">Silinəcək entity-lərin ID-lərinin siyahısı.</param>
    public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
    {
        if (ids == null || !ids.Any())
            return Task.CompletedTask; // Boş və ya null siyahı olduqda heç bir əməliyyat etmə.

        // Yalnız Entity<Guid> tipləri üçün uyğunluq yoxlaması və çevirmə
        if (typeof(TEntity).IsAssignableTo(typeof(Entity<Guid>)))
        {
            _context.RemoveRange(entity.Where(e => ids.Contains((e as Entity<Guid>)!.Id)));
            return SaveChangesAsync();
        }

        throw new InvalidOperationException("BulkDeleteById is only supported for entities implementing Entity<Guid>.");
    }


    /// <summary>
    /// Verilən entity-ləri toplu şəkildə verilənlər bazasında yeniləyir.
    /// </summary>
    /// <param name="entities">Yenilənəcək entity-lərin siyahısı.</param>
    public virtual Task BulkUpdate(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return Task.CompletedTask; // Boş və ya null siyahı olduqda heç bir əməliyyat etmə.

        entity.UpdateRange(entities);
        return SaveChangesAsync();
    }

    #endregion

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public int SaveChanges()
        => _context.SaveChanges();
}
