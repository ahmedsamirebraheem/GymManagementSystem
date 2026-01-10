using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementDataAccessLayer.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includeProperties);

    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null);
    //with params
    Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includeProperties);

    //with string
    Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
             string? includeProperties = null);

    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    
}
