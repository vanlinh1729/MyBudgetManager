using LinqKit;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;

public class GenericRepository<T> : IGenericRepositoryAsync<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
        
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();    
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task BulkInsertAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
    {
        return await _dbContext.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();    
    }

public async Task<IReadOnlyList<T>> GetPagedAdvancedReponseAsync(int pageNumber, int pageSize, string orderBy, string fields, ExpressionStarter<T> predicate)
{
    var query = _dbContext.Set<T>().AsExpandable().Where(predicate);
    
    // Apply dynamic ordering if specified
    if (!string.IsNullOrEmpty(orderBy))
    {
        query = query.OrderBy(orderBy);
    }

    // Apply pagination
    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

    // Select specific fields if specified
    if (!string.IsNullOrEmpty(fields))
    {
        query = query.Select<T>("new(" + fields + ")");
    }

    return await query.ToListAsync();
}

public async Task<IReadOnlyList<dynamic>> GetAllShapeAsync(string orderBy, string fields)
{
    var query = _dbContext.Set<dynamic>().AsQueryable();

    // Sắp xếp nếu có `orderBy`
    if (!string.IsNullOrEmpty(orderBy))
    {
        query = query.OrderBy(orderBy);
    }

    // Chọn các trường nếu `fields` không rỗng
    if (!string.IsNullOrEmpty(fields))
    {
        query = query.Select($"new({fields})").Cast<dynamic>();
    }

    return await query.ToListAsync();
}

}