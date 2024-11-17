using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Lemon.Backend.Entities;

namespace Lemon.Backend.Services;

public interface IBaseRepository<T> where T : BaseEntity
{
    public DbContext DbCtx { get; set; }

    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> GetByGuidsAsync(IEnumerable<Guid> guids);
    Task<T?> GetByIdAsync(Guid guid);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveAsync();
}