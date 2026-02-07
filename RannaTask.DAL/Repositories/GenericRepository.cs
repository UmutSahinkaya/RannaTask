using Microsoft.EntityFrameworkCore;
using RannaTask.DAL.Contexts;
using RannaTask.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories
{
    public class GenericRepository<T, TId>: IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct
    {
        protected AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public Task<bool> AnyAsync(TId id) => _dbSet.AnyAsync(x => x.Id.Equals(id));

        // Soft delete filter: Only return non-deleted items
        public IQueryable<T> GetAll() => _dbSet.AsQueryable().Where(x => !x.IsDeleted).AsNoTracking();

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).Where(x => !x.IsDeleted).AsNoTracking();

        public ValueTask<T?> GetByIdAsync(int id) => _dbSet.FindAsync(id);

        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        // Hard delete (for internal use)
        public void Delete(T entity) => _dbSet.Remove(entity);

        // Soft delete
        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }
    }
}
