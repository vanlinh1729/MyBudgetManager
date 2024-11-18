using LinqKit;

namespace MyBudgetManagement.Domain.Interfaces;

public interface IGenericRepositoryAsync<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);

    Task<IReadOnlyList<T>> GetAllAsync();

    Task<T> AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);
    
    Task BulkInsertAsync(IEnumerable<T> entities);

    Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);

    Task<IReadOnlyList<T>> GetPagedAdvancedReponseAsync(int pageNumber, int pageSize, string orderBy, string fields, ExpressionStarter<T> predicate);

    Task<IReadOnlyList<dynamic>> GetAllShapeAsync(string orderBy, string fields);
}