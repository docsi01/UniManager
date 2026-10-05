using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly IPersonLogic<Teacher> _teacherLogic;

        public TeachersController(IPersonLogic<Teacher> teacherLogic)
        {
            _teacherLogic = teacherLogic;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Teacher>> GetAll()
        {
            var teachers = _teacherLogic.ReadAll("Courses");
            return Ok(teachers);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Teacher newTeacher)
        {
            try
            {
                _teacherLogic.Create(newTeacher);
                return Ok("Teacher created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}