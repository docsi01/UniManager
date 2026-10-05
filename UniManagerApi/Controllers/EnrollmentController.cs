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
        public ActionResult<IEnumerable<Enrollment>> GetAll()
        {
            var enrollment = _enrollLogic.ReadAll();
            return Ok(enrollment);
        }

        [HttpPost]
        public ActionResult Enroll([FromBody] Enrollment newEnrollment)
        {
            try
            {
                _enrollLogic.Create(newEnrollment);
                return Ok("Student successfully enrolled in the course!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Enrollment updatedEnrollment)
        {
            if (id != updatedEnrollment.Id)
            {
                return BadRequest("The IDs don't match!");
            }
            try
            {
                _enrollLogic.Update(updatedEnrollment);
                return Ok("Enrollment successfully updated!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
