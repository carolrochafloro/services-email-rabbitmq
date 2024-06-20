using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text.Json.Serialization;

namespace FormContato.Models;
// adicionada FK userId, rodar migration antes de testar. ajustar home controller para salvar userId
public class ContactModel
{
    public Guid Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Message { get; set; }

    public bool? IsSent { get; set; } = false;
    public DateTime? SentTimestamp { get; private set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserModel.Id))]
    public Guid UserId { get; set; }

    [Required]
    public string SentTo { get; set; }

    [JsonIgnore]
    public UserModel User { get; set; }

}
