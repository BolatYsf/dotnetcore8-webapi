using App.Application.Contracts.Persistence;
using App.Domain.Entities.Common;
using App.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistence
{
    public class GenericRepository<T,TId>(AppDbContext dbContext) : IGenericRepository<T,TId> where T : BaseEntity<TId> where TId : struct
    {
        protected AppDbContext Context = dbContext;

        private readonly DbSet<T> _dbSet=dbContext.Set<T>();

        // if i gonna manipule this data . i can use assnotracking so this data will not be storage in ram
        public async ValueTask AddAsync(T entity)=> await _dbSet.AddAsync(entity);

        public void Delete(T entity)=> _dbSet.Remove(entity);

        public IQueryable<T> GetAll()=> _dbSet.AsQueryable().AsNoTracking();

        public ValueTask<T?> GetByIdAsync(int id)=>_dbSet.FindAsync(id);

        public void Update(T entity)=>_dbSet.Update(entity);

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();

        public Task<List<T>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            return _dbSet.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate);
        }

        public Task<bool> AnyAsync(TId id)
        {
            return _dbSet.AllAsync(x=>x.Id.Equals(id));
         }
    }
}
