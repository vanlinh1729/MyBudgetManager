using System.Collections;
using LinqKit;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;

    public GenericRepositoryAsync(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        // Không gọi SaveChanges ở đây
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        // Không gọi SaveChanges ở đây
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        // Không gọi SaveChanges ở đây
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task BulkInsertAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        // Không gọi SaveChanges ở đây
    }

    public Task<Domain.Common.PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int pageNumber = 1, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
    {
        return await _dbContext.Set<T>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetPagedAdvancedReponseAsync(
        int pageNumber, 
        int pageSize, 
        string orderBy, 
        string fields, 
        ExpressionStarter<T> predicate)
    {
        var query = _dbContext.Set<T>().AsExpandable().Where(predicate);
        
        if (!string.IsNullOrEmpty(orderBy))
        {
            query = query.OrderBy(orderBy);
        }

        if (!string.IsNullOrEmpty(fields))
        {
            query = query.Select<T>($"new({fields})");
        }

        return await query
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<dynamic>> GetAllShapeAsync(string orderBy, string fields)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (!string.IsNullOrEmpty(orderBy))
        {
            query = query.OrderBy(orderBy);
        }

        if (!string.IsNullOrEmpty(fields))
        {
            return await query.Select($"new({fields})").ToDynamicListAsync();
        }

        return await query.Cast<dynamic>().ToListAsync();
    }

    // Thêm các phương thức hỗ trợ UoW
    public IQueryable<T> GetQueryable()
    {
        return _dbContext.Set<T>().AsQueryable();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
    {
        return predicate == null 
            ? await _dbContext.Set<T>().CountAsync()
            : await _dbContext.Set<T>().CountAsync(predicate);
    }
}