using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;

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
        public async Task<ActionResult> Read(int id)
        {
            if (id <= 0) { return BadRequest("The ID must be greater than 0"); }
            try
            {
                await _courseLogic.ReadAsync(id);
                return Ok("Course created successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Course newCourse)
        {
            try
            {
                await _courseLogic.CreateAsync(newCourse);
                return Ok("Course created successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Course updatedCourse)
        {
            if (id != updatedCourse.Id) { return BadRequest($"The IDs don't match!"); }
            try
            {
                await _courseLogic.UpdateAsync(updatedCourse);
                return Ok("Course successfully updated!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}
