using System.ComponentModel.DataAnnotations;

namespace FormContato.DTOs;

public class ContactDTO
{
    public Guid Id { get; set; }
    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Mensagem { get; set; }
    [Required]
    public bool IsSent { get; set; }
    [Required]
    public string? SentTo { get; set; }

}
