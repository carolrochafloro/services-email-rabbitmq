using System.ComponentModel.DataAnnotations;

namespace FormContato.Models;

public class ContactViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Mensagem { get; set; }

    public bool IsSent { get; set; };
}
