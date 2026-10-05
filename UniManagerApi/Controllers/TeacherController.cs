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

        [HttpGet("all")]
        public ActionResult<IEnumerable<Teacher>> GetAll()
        {
            var teachers = _teacherLogic.ReadAll("Courses");
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public ActionResult<Teacher> Read(int id)
        {
            if (id <= 0)
            {
                return BadRequest("The ID must be greater than 0!");
            }
            try
            {
                var teachers = _teacherLogic.Read(id);
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Teacher updatedTeacher)
        {
            if (id != updatedTeacher.Id)
            {
                return BadRequest("The IDs don't match!");
            }
            try
            {
                _teacherLogic.Update(updatedTeacher);
                return Ok("Teacher updated successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}