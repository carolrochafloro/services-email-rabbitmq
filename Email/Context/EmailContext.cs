using FormContato.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Context;
public class EmailContext : DbContext
{
    public EmailContext(DbContextOptions<EmailContext> options) : base(options)
    {

    }
    public DbSet<ContactViewModel> Contacts { get; set; }

}
