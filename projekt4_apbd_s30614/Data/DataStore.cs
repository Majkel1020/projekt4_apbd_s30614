using projekt4_apbd_s30614.Models;

namespace projekt4_apbd_s30614.Data;

public static class DataStore
{
    public static List<Room> Rooms { get; } = new()
    {
        new Room { Id = 1, Name = "Sala 101", BuildingCode = "A", Floor = 1, Capacity = 30, HasProjector = true,  IsActive = true  },
        new Room { Id = 2, Name = "Sala 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true,  IsActive = true  },
        new Room { Id = 3, Name = "Lab 305",  BuildingCode = "B", Floor = 3, Capacity = 16, HasProjector = false, IsActive = true  },
        new Room { Id = 4, Name = "Sala 002", BuildingCode = "A", Floor = 0, Capacity = 50, HasProjector = true,  IsActive = true  },
        new Room { Id = 5, Name = "Sala 110", BuildingCode = "C", Floor = 1, Capacity = 20, HasProjector = false, IsActive = false },
    };

    public static List<Reservation> Reservations { get; } = new()
    {
        new Reservation
        {
            Id = 1, RoomId = 1, OrganizerName = "Anna Kowalska",
            Topic = "Warsztaty z HTTP i REST",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 30),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2, RoomId = 1, OrganizerName = "Piotr Nowak",
            Topic = "Wprowadzenie do ASP.NET Core",
            Date = new DateOnly(2026, 5, 10),
            StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(15, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3, RoomId = 2, OrganizerName = "Marta Wiśniewska",
            Topic = "Docker i konteneryzacja",
            Date = new DateOnly(2026, 5, 12),
            StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4, RoomId = 3, OrganizerName = "Tomasz Zając",
            Topic = "Bazy danych — SQL zaawansowany",
            Date = new DateOnly(2026, 5, 13),
            StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 30),
            Status = "planned"
        },
        new Reservation
        {
            Id = 5, RoomId = 4, OrganizerName = "Katarzyna Lewandowska",
            Topic = "Scrum i Agile w praktyce",
            Date = new DateOnly(2026, 5, 14),
            StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 6, RoomId = 2, OrganizerName = "Adam Wróbel",
            Topic = "Git zaawansowany",
            Date = new DateOnly(2026, 5, 15),
            StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0),
            Status = "cancelled"
        },
    };

    public static int NextRoomId() => Rooms.Count == 0 ? 1 : Rooms.Max(r => r.Id) + 1;
    public static int NextReservationId() => Reservations.Count == 0 ? 1 : Reservations.Max(r => r.Id) + 1;
}
