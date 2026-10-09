using System;
using System.Collections.Generic;
using System.Text;
using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManager.Logic
{
    public interface IStudentLogic
    {
        //async
        Task<Student> CreateAsync(StudentCreateDto student);
        Task<IEnumerable<Student>> ReadAllAsync(string includeProperties="");
        Task <Student> ReadAsync(int id);
        Task UpdateAsync(int id, StudentUpdateDto student);
        Task DeleteAsync(int id);
    }
}
