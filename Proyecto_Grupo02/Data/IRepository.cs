using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> ToListAsync(IQueryable<T> query);
        void Add(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();
    }
}