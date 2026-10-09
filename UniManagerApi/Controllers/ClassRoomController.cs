using Microsoft.AspNetCore.Mvc;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : ControllerBase
    {
        private readonly IClassRoomLogic _classroomLogic;

        public ClassRoomController(IClassRoomLogic classroomLogic)
        {
            _classroomLogic = classroomLogic;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ClassRoom>>> GetAll()
        {
            var classRoom = await _classroomLogic.ReadAllAsync();
            return Ok(classRoom);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassRoom>> Read(int id)
        {
            if (id < 1) { return BadRequest("ID must be greater than 0!"); }
            try
            {
                var classRoom = await _classroomLogic.ReadAsync(id);
                if (classRoom == null)
                {
                    return NotFound($"No classroom found with this ID: {id}");
                }
                return Ok(classRoom);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ClassRoomCreateDto newClassRoom)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var createdClassRoom = await _classroomLogic.CreateAsync(newClassRoom);
            return CreatedAtAction(nameof(Read), new { id = createdClassRoom.Id }, createdClassRoom);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ClassRoomUpdateDto updatedClassRoom)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                await _classroomLogic.UpdateAsync(id, updatedClassRoom);
                return Ok("ClassRoom updated successfully!");
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _classroomLogic.DeleteAsync(id);
                return Ok("ClassRoom deleted successfully!");
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}