using Email.Context;
using FormContato.Models;


namespace Email.Services;
public class UpdateDB
{
    private readonly EmailContext _context;
    private readonly ILogger<UpdateDB> _logger;

    public UpdateDB(EmailContext context, ILogger<UpdateDB> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task UpdateIsSent(bool response, Guid id)
    {
 
        

        try
        {
            var contact = await _context.Set<ContactModel>().FindAsync(id);

            if (contact is null)
            {
                throw new Exception("----- Error: contact not found. -----");
            }

            if (response)
            {
                contact.IsSent = true;
            }
            else
            {
                contact.IsSent = false;
            }

            _context.Update(contact);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Contact Id {contact.Id} updated.");

        } catch (Exception ex)
        {

            _logger.LogError($"----- Error on UpdateDB: {ex}");

        }
       

    }
}
