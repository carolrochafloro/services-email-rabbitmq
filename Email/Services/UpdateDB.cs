using Email.Context;
using FormContato.Models;


namespace Email.Services;
internal class UpdateDB
{
    private readonly EmailContext _context;

    public UpdateDB(EmailContext context)
    {
        _context = context;
    }

    public async Task UpdateIsSent(bool response, Guid id)
    {

        var user = await _context.Set<ContactViewModel>().FindAsync(id);

        if (user is null)
        {
            throw new Exception("User not found.");
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
        _context.SaveChanges();

        await Console.Out.WriteLineAsync($"Contact Id {user.Id} updated.");

    }
}
