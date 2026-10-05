using Microsoft.EntityFrameworkCore;

namespace UniManager.Repository
{
    public sealed class GenericRepo<T> : IRepository<T> where T : class
    {
        private readonly UniDbContext _ctx;
        private readonly DbSet<T> _dbSet;

        public GenericRepo(UniDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = ctx.Set<T>();
        }

        public IEnumerable<T> ReadAll(string includeProperties="")
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) { 
                    query = query.Include(includeProperty);
                }
            }
            return query.ToList();
        }

        public T Read(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) throw new KeyNotFoundException($"{typeof(T).Name} does not exist!");
            return entity;
        }

        public void Create(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Add(entity);
            _ctx.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _ctx.SaveChanges();
        }

        public void Delete(int id) { 
            var entity = _dbSet.Find(id);
            if (entity != null) {
                _dbSet.Remove(entity);
                _ctx.SaveChanges();
            }
        }
    }
}
