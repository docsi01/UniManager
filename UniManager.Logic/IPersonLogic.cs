namespace UniManager.Logic
{
    public interface IPersonLogic<T> where T : class
    {
        IEnumerable<T> ReadAll();
        T Read(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
