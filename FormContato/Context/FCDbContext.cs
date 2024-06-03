using FormContato.Models;
using Microsoft.EntityFrameworkCore;

namespace FormContato.Context;

public class FCDbContext : DbContext
{
    public FCDbContext(DbContextOptions<FCDbContext> options) : base(options)
    {

    }
    public DbSet<ContactViewModel> Contacts { get; set; }
    public DbSet<LogModel> Logs { get; set; }
}
