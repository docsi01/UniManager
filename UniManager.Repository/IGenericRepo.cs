namespace UniManager.Repository
{
    public interface IGenericRepo<T> where T : class
    {
        IEnumerable<T> ReadAll(string inculeProperties = "");
        Task<IEnumerable<T>> ReadAllAsync(string includeProperties = "");
        T Read(int id);
        Task<T> ReadAsync(int id);
        void Create(T entity);
        Task<T> CreateAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(int id);
        Task DeleteAsync(int id);
    }
}
