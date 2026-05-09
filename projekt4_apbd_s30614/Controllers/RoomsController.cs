using Microsoft.AspNetCore.Mvc;
using projekt4_apbd_s30614.Data;
using projekt4_apbd_s30614.Models;

namespace projekt4_apbd_s30614.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    // GET /api/rooms
    // GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = DataStore.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            rooms = rooms.Where(r => r.IsActive);

        return Ok(rooms.ToList());
    }

    // GET /api/rooms/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        return Ok(room);
    }

    // GET /api/rooms/building/{buildingCode}
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = DataStore.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    // POST /api/rooms
    [HttpPost]
    public IActionResult Create([FromBody] Room room)
    {
        room.Id = DataStore.NextRoomId();
        DataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    // PUT /api/rooms/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Room updated)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        room.Name = updated.Name;
        room.BuildingCode = updated.BuildingCode;
        room.Floor = updated.Floor;
        room.Capacity = updated.Capacity;
        room.HasProjector = updated.HasProjector;
        room.IsActive = updated.IsActive;

        return Ok(room);
    }

    // DELETE /api/rooms/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound($"Room with id {id} not found.");

        var today = DateOnly.FromDateTime(DateTime.Today);
        bool hasFutureReservations = DataStore.Reservations
            .Any(r => r.RoomId == id && r.Date >= today);

        if (hasFutureReservations)
            return Conflict($"Cannot delete room {id} — it has future or ongoing reservations.");

        DataStore.Rooms.Remove(room);
        return NoContent();
    }
}
