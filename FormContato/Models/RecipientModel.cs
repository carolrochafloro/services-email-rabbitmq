using System.ComponentModel.DataAnnotations;

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
}
