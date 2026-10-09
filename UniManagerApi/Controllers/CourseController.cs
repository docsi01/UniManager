using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseLogic _courseLogic;

        public CourseController(ICourseLogic courseLogic)
        {
            _courseLogic = courseLogic;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Course>>> GetAll()
        {
            var courses = await _courseLogic.ReadAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> Read(int id)
        {
            if (id <= 0) { return BadRequest("The ID must be greater than 0"); }
            try
            {
                var course = await _courseLogic.ReadAsync(id);
                if (course == null) { return NotFound($"No course found with this ID: {id}"); }
                return Ok(course);
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CourseCreateDto newCourse)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                var createdCourse = await _courseLogic.CreateAsync(newCourse);
                return CreatedAtAction(nameof(Read), new { id = createdCourse.Id }, createdCourse);
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CourseUpdateDto updatedCourse)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                await _courseLogic.UpdateAsync(id,updatedCourse);
                return Ok("Course successfully updated!");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _courseLogic.DeleteAsync(id);
                return Ok("Course deleted successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}
