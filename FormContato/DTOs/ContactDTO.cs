using System.ComponentModel.DataAnnotations;

namespace FormContato.DTOs;

public class ContactDTO
{
    public Guid Id { get; set; }
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Message { get; set; }
    [Required]
    public bool IsSent { get; set; }
    [Required]
    public string? SentTo { get; set; }

}
