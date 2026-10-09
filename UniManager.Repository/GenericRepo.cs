using Microsoft.EntityFrameworkCore;

namespace UniManager.Repository
{
    public sealed class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly UniDbContext _ctx;
        private readonly DbSet<T> _dbSet;

        public GenericRepo(UniDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = ctx.Set<T>();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public IEnumerable<T> ReadAll(string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
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

        public async Task<T?> ReadAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Create(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Add(entity);
            _ctx.SaveChanges();
        }

        public async Task<T> CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _dbSet.AddAsync(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _ctx.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _ctx.SaveChanges();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _ctx.SaveChangesAsync();
            }
            
        }
    }
}
