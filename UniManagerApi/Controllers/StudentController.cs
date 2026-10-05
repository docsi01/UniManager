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

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            var students = _studentLogic.ReadAll("Enrollments.Course.Teacher");
            return Ok(students);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Student newStudent)
        {
            try
            {
                newStudent.EnrollmentDate = DateTime.Now;
                _studentLogic.Create(newStudent);
                return Ok("Student created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
