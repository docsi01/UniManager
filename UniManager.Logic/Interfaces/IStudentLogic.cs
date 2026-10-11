using System;
using System.Collections.Generic;
using System.Text;
using UniManager.Models.DTOs;
using UniManager.Models.Models;

namespace UniManager.Logic.Interfaces
{
    public interface IStudentLogic
    {
        //async
        Task<Student> CreateAsync(StudentCreateDto student);
        Task<IEnumerable<Student>> ReadAllAsync(string includeProperties="");
        Task<Student?> ReadAsync(int id);
        Task UpdateAsync(int id, StudentUpdateDto student);
        Task DeleteAsync(int id);
    }
}
