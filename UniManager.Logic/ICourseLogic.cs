namespace UniManager.Logic
{
    public interface ICourseLogic<T> where T : class
    {

        IEnumerable<T> ReadAll();
        T Read(int id);
        void Create(T course);
        void Update(T course);
        void Delete(int id);
    }
}
