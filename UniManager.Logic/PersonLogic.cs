using UniManager.Repository;

namespace UniManager.Logic
{
    public class PersonLogic<T> : IPersonLogic<T> where T : class
    {
        private readonly IRepository<T> _repo;

        public PersonLogic(IRepository<T> repo)
        {
            _repo = repo;
        }

        public IEnumerable<T> ReadAll(string includeProperties ="")
        {
            return _repo.ReadAll(includeProperties);
        }

        public T Read(int id)
        {
            return _repo.Read(id);
        }

        public void Create(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _repo.Create(entity);
        }
        public void Update(T entity)
        {
            _repo.Update(entity);
        }
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
