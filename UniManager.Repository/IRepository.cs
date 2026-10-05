using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Repository
{
    public interface IRepository<T>where T : class
    {
        IEnumerable<T> ReadAll(string inculeProperties ="");
        T Read(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
