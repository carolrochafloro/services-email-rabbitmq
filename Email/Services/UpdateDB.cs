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

        var user = await _context.Set<ContactViewModel>().FindAsync(id);

        if (user is null)
        {
            throw new Exception("----- Error: User not found. -----");
        }

        if (response)
        {
            user.IsSent = true;
        }
        else
        {
            user.IsSent = false;
        }

        _context.Update(user);

        try
        {

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Contact Id {user.Id} updated.");

        } catch (Exception ex)
        {

            _logger.LogError($"----- Error on UpdateDB: {ex}");

        }
       

    }
}
