using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FormContato.Models;

public class RecipientModel
{
    public Guid Id { get; set; }

    [Required]
    public string RecipientEmail { get; set; }

    [Required]
    public string Url { get; set;}
    public string ShortUrl { get; set;}

    [Required]
    [ForeignKey(nameof(UserModel.Id))]
    public Guid UserId { get; set; }

    [JsonIgnore]
    public UserModel User { get; set; }

}
