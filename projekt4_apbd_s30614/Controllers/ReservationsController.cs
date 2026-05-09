using Microsoft.AspNetCore.Mvc;
using projekt4_apbd_s30614.Data;
using projekt4_apbd_s30614.Models;

namespace projekt4_apbd_s30614.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    // GET /api/reservations
    // GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = DataStore.Reservations.AsEnumerable();

        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value);

        if (!string.IsNullOrWhiteSpace(status))
            reservations = reservations.Where(r =>
                r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value);

        return Ok(reservations.ToList());
    }

    // GET /api/reservations/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        return Ok(reservation);
    }

    // POST /api/reservations
    [HttpPost]
    public IActionResult Create([FromBody] Reservation reservation)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
            return NotFound($"Room with id {reservation.RoomId} not found.");

        if (!room.IsActive)
            return Conflict($"Room {reservation.RoomId} is inactive and cannot be reserved.");

        if (HasTimeConflict(reservation.RoomId, reservation.Date, reservation.StartTime, reservation.EndTime, excludeId: null))
            return Conflict("The requested time slot overlaps with an existing reservation for this room.");

        reservation.Id = DataStore.NextReservationId();
        DataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    // PUT /api/reservations/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Reservation updated)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == updated.RoomId);
        if (room is null)
            return NotFound($"Room with id {updated.RoomId} not found.");

        if (!room.IsActive)
            return Conflict($"Room {updated.RoomId} is inactive and cannot be reserved.");

        if (HasTimeConflict(updated.RoomId, updated.Date, updated.StartTime, updated.EndTime, excludeId: id))
            return Conflict("The updated time slot overlaps with an existing reservation for this room.");

        reservation.RoomId = updated.RoomId;
        reservation.OrganizerName = updated.OrganizerName;
        reservation.Topic = updated.Topic;
        reservation.Date = updated.Date;
        reservation.StartTime = updated.StartTime;
        reservation.EndTime = updated.EndTime;
        reservation.Status = updated.Status;

        return Ok(reservation);
    }

    // DELETE /api/reservations/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound($"Reservation with id {id} not found.");

        DataStore.Reservations.Remove(reservation);
        return NoContent();
    }

    private static bool HasTimeConflict(int roomId, DateOnly date, TimeOnly start, TimeOnly end, int? excludeId)
    {
        return DataStore.Reservations
            .Where(r => r.RoomId == roomId && r.Date == date)
            .Where(r => excludeId == null || r.Id != excludeId)
            .Any(r => start < r.EndTime && end > r.StartTime);
    }
}
