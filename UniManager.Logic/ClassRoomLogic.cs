using UniManager.Models;
using UniManager.Models.DTOs;
using UniManager.Repository;

namespace UniManager.Logic
{
    public sealed class ClassRoomLogic : IClassRoomLogic
    {
        private readonly IGenericRepo<ClassRoom> _repo;
        public ClassRoomLogic(IGenericRepo<ClassRoom> repo)
        {
            _repo = repo;
        }
        public async Task<ClassRoom> CreateAsync(ClassRoomCreateDto dto)
        {
            var newClassRoom = new ClassRoom
            {
                RoomName = dto.RoomName,
                Capacity = dto.Capacity,
            };
            return await _repo.CreateAsync(newClassRoom);
        }

        public async Task<IEnumerable<ClassRoom>> ReadAllAsync(string includeProperties = "")
        {
            return await _repo.ReadAllAsync(includeProperties);
        }
        public async Task<ClassRoom?> ReadAsync(int id)
        {
            return await _repo.ReadAsync(id);
        }

        public async Task UpdateAsync(int id, ClassRoomUpdateDto dto)
        {
            var existingClassRoom = await _repo.ReadAsync(id);
            if (existingClassRoom == null) { throw new KeyNotFoundException($"Classroom with this ID ({id}) not found!"); }
            existingClassRoom.Capacity = dto.Capacity;
            existingClassRoom.RoomName = dto.RoomName;
            await _repo.UpdateAsync(existingClassRoom);
        }
        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
