using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManager.Logic
{
    public interface IEnrollmentLogic
    {
        Task<Enrollment> CreateAsync(EnrollmentCreateDto enrollment);
        Task<IEnumerable<Enrollment>> ReadAllAsync(string includeProperties = "");
        Task<Enrollment> ReadAsync(int id);
        Task UpdateAsync(int id, EnrollmentUpdateDto enrollment);
        Task DeleteAsync(int id);
    }
}
