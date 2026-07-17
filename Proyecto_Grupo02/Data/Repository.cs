using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CatalogoDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(CatalogoDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) =>
            _dbSet.Where(predicate).FirstOrDefaultAsync();

        public Task<List<T>> ToListAsync(IQueryable<T> query) => query.ToListAsync();

        public void Add(T entity) => _dbSet.Add(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}