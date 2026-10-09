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
            try
            {
                var enrollment = await _enrollLogic.ReadAsync(id);
                return Ok(enrollment);
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EnrollmentCreateDto newEnrollment)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                var createdEnrollment = await _enrollLogic.CreateAsync(newEnrollment);
                return CreatedAtAction(nameof(Read), new { id = createdEnrollment.Id }, createdEnrollment);
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EnrollmentUpdateDto updatedEnrollment)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                await _enrollLogic.UpdateAsync(id, updatedEnrollment);
                return Ok("Enrollment successfully updated!");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
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
