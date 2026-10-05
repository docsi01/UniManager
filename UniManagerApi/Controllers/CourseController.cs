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

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetAll()
        {
            var courses = _courseLogic.ReadAll();
            return Ok(courses);
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
    }
}
