using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentLogic _enrollLogic;

        public EnrollmentController(IEnrollmentLogic enrollLogic)
        {
            _enrollLogic = enrollLogic;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetAll()
        {
            var enrollment = await _enrollLogic.ReadAllAsync();
            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<ActionResult> Enroll([FromBody] Enrollment newEnrollment)
        {
            try
            {
                await _enrollLogic.CreateAsync(newEnrollment);
                return Ok("Student successfully enrolled in the course!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Enrollment updatedEnrollment)
        {
            if (id != updatedEnrollment.Id) { return BadRequest("The IDs don't match!"); }
            try
            {
                await _enrollLogic.UpdateAsync(updatedEnrollment);
                return Ok("Enrollment successfully updated!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
