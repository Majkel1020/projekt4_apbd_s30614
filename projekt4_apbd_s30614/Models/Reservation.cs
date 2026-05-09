using System.ComponentModel.DataAnnotations;

namespace projekt4_apbd_s30614.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "RoomId must be greater than zero.")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "OrganizerName is required.")]
    [MaxLength(150)]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Topic is required.")]
    [MaxLength(200)]
    public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(planned|confirmed|cancelled)$",
        ErrorMessage = "Status must be 'planned', 'confirmed' or 'cancelled'.")]
    public string Status { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
            yield return new ValidationResult(
                "EndTime must be later than StartTime.",
                new[] { nameof(EndTime) });
    }
}
