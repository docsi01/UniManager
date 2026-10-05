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

        [HttpGet("all")]
        public ActionResult<IEnumerable<ClassRoom>> GetAll()
        {
            var classRoom=_classroomLogic.ReadAll();
            return Ok(classRoom);
        }

        [HttpGet("{id}")]
        public ActionResult<ClassRoom> Read(int id)
        {
            if (id < 1)
            {
                return BadRequest("ID must be greater than 0!");
            }
            try
            {
                var classRoom = _classroomLogic.Read(id);
                if (classRoom == null)
                {
                    return NotFound($"No classroom found with this ID: {id}");
                }
                return Ok(classRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] ClassRoom updatedClassRoom)
        {
            if (id != updatedClassRoom.Id)
            {
                return BadRequest("The IDs don't match!");
            }
            try
            {
                _classroomLogic.Update(updatedClassRoom);
                return Ok("ClassRoom updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}