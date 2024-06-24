using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FormContato.DTOs;

public class ContactDTO
{
    public Guid Id { get; set; }

    [DisplayName("Sent at")]
    public DateTime SentTimestamp { get; set; }

    [Required]
    [DisplayName("From")]
    public string? Name { get; set; }

    [Required]
    [DisplayName("E-mail")]
    public string? Email { get; set; }

    [Required]
    public string? Message { get; set; }
    [Required]
    [DisplayName("Status")]
    public bool IsSent { get; set; }
    [Required]
    [DisplayName("To")]
    public string? SentTo { get; set; }

}
