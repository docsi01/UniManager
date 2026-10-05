using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseLogic<Course> _courseLogic;

        public CourseController(ICourseLogic<Course> courseLogic)
        {
            _courseLogic = courseLogic;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<Course>> GetAll()
        {
            var courses = _courseLogic.ReadAll();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public ActionResult<Course> Read(int id)
        {
            if (id <= 0)
            {
                return BadRequest("The ID must be greater than 0");
            }
            try
            {
                var courses = _courseLogic.Read(id);
                return Ok(courses);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public ActionResult Create([FromBody] Course newCourse)
        {
            try
            {
                _courseLogic.Create(newCourse);
                return Ok("Course created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Course updatedCourse)
        {
            if (id != updatedCourse.Id)
            {
                return BadRequest($"The IDs don't match!");
            }
            try
            {
                _courseLogic.Update(updatedCourse);
                return Ok("Course successfully updated!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
