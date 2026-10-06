using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Models.DTOs;

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
        public async Task<ActionResult> Enroll([FromBody] EnrollmentCreateDto newEnrollment)
        {
            try
            {
                await _enrollLogic.CreateAsync(newEnrollment);
                return Ok("Student successfully enrolled in the course!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EnrollmentUpdateDto updatedEnrollment)
        {
            try
            {
                await _enrollLogic.UpdateAsync(id,updatedEnrollment);
                return Ok("Enrollment successfully updated!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
