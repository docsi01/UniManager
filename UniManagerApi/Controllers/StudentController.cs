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
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            var students = _studentLogic.ReadAll("Enrollments.Course.Teacher");
            return Ok(students);
        }

        [HttpGet("{id}")]
        public ActionResult<Student>Read(int id)
        {
            var students =_studentLogic.Read(id);
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

        [HttpPut]
        public ActionResult Update(int id, [FromBody] Student updatedStudent)
        {
            if (id != updatedStudent.Id)
            {
                return BadRequest("The IDs don't match");
            }
            try
            {
                _studentLogic.Update(updatedStudent);
                return Ok("Student updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
