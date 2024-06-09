using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FormContato.Models;

public class UserModel
{
    public Guid Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string Salt { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastUpdatedAt { get; set;}
    public string? LastUpdatedBy { get; set; }
}
