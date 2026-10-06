using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IPersonLogic<Student> _studentLogic;

        public StudentController(IPersonLogic<Student> studentLogic)
        {
            _studentLogic = studentLogic;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _studentLogic.ReadAllAsync("Enrollments.Course.Teacher");
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Read(int id)
        {
            if (id <= 0) { return BadRequest("The ID must be greater then 0!"); }
            try
            {
                var students = await _studentLogic.ReadAsync(id);
                return Ok(students);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Student newStudent)
        {
            try
            {
                newStudent.EnrollmentDate = DateTime.Now;
                await _studentLogic.CreateAsync(newStudent);
                return Ok("Student created successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPut]
        public async Task<ActionResult> Update(int id, [FromBody] Student updatedStudent)
        {
            if (id != updatedStudent.Id) { return BadRequest("The IDs don't match"); }
            try
            {
                await _studentLogic.UpdateAsync(updatedStudent);
                return Ok("Student updated successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}
