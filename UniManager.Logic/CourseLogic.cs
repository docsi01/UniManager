using UniManager.Repository;

namespace UniManager.Logic
{
    public sealed class CourseLogic<T> : ICourseLogic<T> where T : class
    {
        private readonly IRepository<T> _repo;
        public CourseLogic(IRepository<T> repo)
        {
            _repo = repo;
        }
        public IEnumerable<T> ReadAll()
        {
            return _repo.ReadAll();
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
