using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Models;
internal class Message
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Mensagem { get; set; }
    public bool IsSent { get; set; }

}
