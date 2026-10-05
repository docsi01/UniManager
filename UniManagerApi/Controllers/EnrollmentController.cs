using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Repository;

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

        [HttpGet]
        public ActionResult<IEnumerable<Enrollment>> GetAll()
        {
            return Ok(_enrollLogic.ReadAll());
        }

        [HttpPost]
        public ActionResult Enroll([FromBody] Enrollment newEnrollment)
        {
            try
            {
                _enrollLogic.Create(newEnrollment);
                return Ok("Student successfully enrolled in the course!");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
