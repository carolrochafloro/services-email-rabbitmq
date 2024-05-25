using System.ComponentModel.DataAnnotations;

namespace FormContato.Models;

public class ContactViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Mensagem { get; set; }
}
