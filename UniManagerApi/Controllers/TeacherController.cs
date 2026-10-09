using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherLogic _teacherLogic;

        public TeachersController(ITeacherLogic teacherLogic)
        {
            _teacherLogic = teacherLogic;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetAll()
        {
            var teachers = await _teacherLogic.ReadAllAsync("Courses");
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> Read(int id)
        {
            if (id <= 0) { return BadRequest("The ID must be greater than 0!"); }
            try
            {
                var teachers = await _teacherLogic.ReadAsync(id);
                return Ok(teachers);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TeacherCreateDto newTeacher)
        {
            var createdTeacher = await _teacherLogic.CreateAsync(newTeacher);
            return CreatedAtAction(nameof(Read), new { id = createdTeacher.Id }, createdTeacher);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TeacherUpdateDto updatedTeacher)
        {
            try
            {
                await _teacherLogic.UpdateAsync(id,updatedTeacher);
                return Ok("Teacher updated successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _teacherLogic.DeleteAsync(id);
                return Ok("Teacher deleted successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}