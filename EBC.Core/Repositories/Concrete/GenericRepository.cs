using System;
using AccountManagerSystem.Repositories.Abstract;
using EBC.Core.IEntities.Common;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Repositories.Concrete;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseEntity
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

    public virtual async Task<int> AddAsync(TEntity entity)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        await this.entity.AddAsync(entity);
        return await SaveChangesAsync();
    }

    public virtual int AddRange(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return 0;

        this.entity.AddRange(entities);
        return SaveChanges();
    }

    public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            return 0;

        await this.entity.AddRangeAsync(entities);
        return await SaveChangesAsync();
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


    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}
