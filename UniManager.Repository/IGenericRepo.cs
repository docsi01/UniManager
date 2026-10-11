namespace UniManager.Repository
{
    public interface IGenericRepo<T> where T : class
    {
        //IEnumerable<T> ReadAll(string inculeProperties = "");
        //T Read(int id);
        //void Create(T entity);
        //void Update(T entity);
        //void Delete(int id);
        
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> ReadAllAsync(string includeProperties = "");
        Task<T?> ReadAsync(int id);
        Task<T?> ReadAsync(int id, string includeProperties);
        void AddWithoutSaving(T entity);
        void UpdateWithoutSaving(T entity);
        Task SaveChangesAsync();
        void ClearChangeTracker();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
