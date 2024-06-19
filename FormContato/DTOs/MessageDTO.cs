using FormContato.Models;

namespace FormContato.DTOs;

public class MessageDTO
{
    public RecipientModel Recipient { get; set; }
    public ContactModel Contact { get; set; }
}
