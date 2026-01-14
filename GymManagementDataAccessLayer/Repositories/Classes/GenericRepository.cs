using GymManagementDataAccessLayer.Data.Context;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Classes;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly GymDBContext _dbContext;
    internal DbSet<T> _dbSet;
    public GenericRepository(GymDBContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;

        if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(e => !((BaseEntity)(object)e).IsDeleted);
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query.AsNoTracking().ToListAsync();
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(e => !((BaseEntity)(object)e).IsDeleted);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.AsNoTracking().ToListAsync();
    }
    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;

        if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(e => !((BaseEntity)(object)e).IsDeleted);
        }

        foreach (var include in includeProperties) query = query.Include(include);

        return await query.FirstOrDefaultAsync(filter);
    }

    // الطريقة اللي بتدعم الـ String Include
    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            query = query.Where(e => !((BaseEntity)(object)e).IsDeleted);
        }

        if (includeProperties != null)
        {
            foreach (var include in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(filter);
    }
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        if (entity is BaseEntity softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
            softDeleteEntity.UpdatedAt = DateTime.Now;
            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }


    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        if (filter != null)
        {
            return await _dbContext.Set<T>().CountAsync(filter);
        }
        return await _dbContext.Set<T>().CountAsync();
    }

    // داخل الـ GenericRepository.cs
    public async Task<T> GetWithDeletedAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(filter);
    }
}
