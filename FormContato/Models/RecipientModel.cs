using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FormContato.Models;

// criar repository, salvar email e final da url no banco de dados
// criar verificação, caso o email já exista retornar a url do banco
// enviar objeto criado com o email p/ produce
// criar controller com a rota recebendo a url criada
// alterar worker para receber e desencriptar email e definir no sendgrid
public class RecipientModel
{
    public Guid Id { get; set; }

    [Required]
    public string RecipientEmail { get; set; }
    [Required]
    public string Url { get; set;}
    [Required]

    [ForeignKey(nameof(UserModel.Id))]
    public Guid UserId { get; set; }
    [JsonIgnore]
    public UserModel AppUser { get; set; }

}
