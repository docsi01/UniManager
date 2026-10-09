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

        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> Read(int id)
        {
            var enrollment = await _enrollLogic.ReadAsync(id);
            if (enrollment == null) return NotFound();
            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EnrollmentCreateDto newEnrollment)
        {
            var createdEnrollment = await _enrollLogic.CreateAsync(newEnrollment);
            return CreatedAtAction(nameof(Read), new { id = createdEnrollment.Id }, createdEnrollment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EnrollmentUpdateDto updatedEnrollment)
        {
            try
            {
                await _enrollLogic.UpdateAsync(id, updatedEnrollment);
                return Ok("Enrollment successfully updated!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _enrollLogic.DeleteAsync(id);
                return Ok("Enrollment deleted successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


    }
}
