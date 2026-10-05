using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : ControllerBase
    {
        private readonly IPersonLogic<ClassRoom> _classroomLogic;

        public ClassRoomController(IPersonLogic<ClassRoom> classroomLogic)
        {
            _classroomLogic = classroomLogic;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClassRoom>> GetAll()
        {
            return Ok(_classroomLogic.ReadAll());
        }

        [HttpPost]
        public ActionResult Create([FromBody] ClassRoom newClassRoom)
        {
            try
            {
                _classroomLogic.Create(newClassRoom);
                return Ok("Classroom created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}