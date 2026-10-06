using System;
using System.Collections.Generic;
using System.Text;
using UniManager.Models;

namespace UniManager.Logic
{
    public interface IEnrollmentLogic
    {
        IEnumerable<Enrollment> ReadAll();
        void Create(Enrollment enrollment);
        void Update(Enrollment enrollment);

        Task<IEnumerable<Enrollment>> ReadAllAsync();
        Task CreateAsync(Enrollment enrollment);
        Task UpdateAsync(Enrollment enrollment);
    }
}
