using UniManager.Repository;

namespace UniManager.Logic
{
    public class PersonLogic<T> : IPersonLogic<T> where T : class
    {
        private readonly IGenericRepo<T> _repo;

        public PersonLogic(IGenericRepo<T> repo)
        {
            _repo = repo;
        }

        public IEnumerable<T> ReadAll(string includeProperties ="")
        {
            return _repo.ReadAll(includeProperties);
        }

        public async Task<IEnumerable<T>> ReadAllAsync(string includeProperties = "")
        {
            return await _repo.ReadAllAsync(includeProperties);
        }

        public T Read(int id)
        {
            return _repo.Read(id);
        }

        public async Task<T> ReadAsync(int id)
        {
            return await (_repo.ReadAsync(id));
        }

        public void Create(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _repo.Create(entity);
        }

        public async Task<T>CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return await _repo.CreateAsync(entity);
        }
        public void Update(T entity)
        {
            _repo.Update(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repo.UpdateAsync(entity);
        }
        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
