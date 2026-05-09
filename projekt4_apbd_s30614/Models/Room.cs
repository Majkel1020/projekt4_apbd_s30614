using System.ComponentModel.DataAnnotations;

namespace projekt4_apbd_s30614.Models;

public class Room
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "BuildingCode is required.")]
    [MaxLength(10)]
    public string BuildingCode { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero.")]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }

    public bool IsActive { get; set; }
}
