using UniManager.Models.DTOs;
using UniManager.Models.Models;

namespace UniManager.Logic.Interfaces
{
    public interface IEnrollmentLogic
    {
        Task<Enrollment> EnrollAsync(EnrollmentCreateDto enrollment);
        Task<IEnumerable<Enrollment>> ReadAllAsync(string includeProperties = "");
        Task<Enrollment> ReadAsync(int id);
        Task UpdateAsync(int id, EnrollmentUpdateDto enrollment);
        Task DropAsync(int id);
    }
}
