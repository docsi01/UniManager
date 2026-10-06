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
        public async Task<ActionResult> Create([FromBody] Teacher newTeacher)
        {
            try
            {
                await _teacherLogic.CreateAsync(newTeacher);
                return Ok("Teacher created successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Teacher updatedTeacher)
        {
            if (id != updatedTeacher.Id) { return BadRequest("The IDs don't match!"); }
            try
            {
                await _teacherLogic.UpdateAsync(updatedTeacher);
                return Ok("Teacher updated successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}