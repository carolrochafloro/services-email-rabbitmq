using System.ComponentModel;

namespace FormContato.DTOs;

public class ProfileDTO
{
    public Guid Id { get; set; }

    [DisplayName("Name")]
    public string Name { get; set; }

    [DisplayName("Last Name")]
    public string LastName { get; set; }

    [DisplayName("E-mail")]
    public string Email { get; set; }

    [DisplayName("Password")]
    public string Password { get; set; }
}
