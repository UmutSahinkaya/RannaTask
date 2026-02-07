using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Repositories
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct
    {
        IQueryable<T> GetAll();
        Task<bool> AnyAsync(TId id);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        ValueTask<T?> GetByIdAsync(int id);
        ValueTask AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
    }
}
