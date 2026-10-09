using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentLogic _studentLogic;

        public StudentController(IStudentLogic studentLogic)
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
                if (students == null)
                {
                    return NotFound($"No student found with this ID: {id}");
                }
                return Ok(students);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] StudentCreateDto newStudent)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var createdStudent = await _studentLogic.CreateAsync(newStudent);
            return CreatedAtAction(nameof(Read), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] StudentUpdateDto updatedStudent)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                await _studentLogic.UpdateAsync(id, updatedStudent);
                return Ok("Student updated successfully!");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _studentLogic.DeleteAsync(id);
                return Ok("Student deleted successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}
