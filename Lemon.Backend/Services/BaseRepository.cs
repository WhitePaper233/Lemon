using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Lemon.Backend.Entities;

namespace Lemon.Backend.Services;

public class BaseRepository<T>(DbContext dbCtx) : IBaseRepository<T> where T : BaseEntity
{
    public DbContext DbCtx { get; set; } = dbCtx;

    public void Create(T entity)
    {
        DbCtx.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        DbCtx.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        DbCtx.Set<T>().Update(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbCtx.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
    {
        return await DbCtx.Set<T>().Where(expression).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetByGuidsAsync(IEnumerable<Guid> guids)
    {
        return await DbCtx.Set<T>().Where(record => guids.Contains(record.Id)).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid guid)
    {
        return await DbCtx.Set<T>().FindAsync(guid);
    }

    public async Task<bool> SaveAsync()
    {
        return await DbCtx.SaveChangesAsync() > 0;
    }
}
