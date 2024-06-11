using FormContato.Models;
using Microsoft.EntityFrameworkCore;

namespace Email.Context;
public class EmailContext : DbContext
{
    public EmailContext(DbContextOptions<EmailContext> options) : base(options)
    {

    }
    public DbSet<ContactModel> Contacts { get; set; }

}
