using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormContato.Migrations
{
    public partial class RenameContacts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome", // nome antigo da coluna
                table: "Contacts", // nome da tabela
                newName: "Name" // novo nome da coluna
            );
            migrationBuilder.RenameColumn(
             name: "Mensagem", // nome antigo da coluna
             table: "Contacts", // nome da tabela
             newName: "Message" // novo nome da coluna
         );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name", // nome novo da coluna
                table: "Contacts", // nome da tabela
                newName: "Nome" // nome antigo da coluna
            );
            migrationBuilder.RenameColumn(
              name: "Message", // nome novo da coluna
              table: "Contacts", // nome da tabela
              newName: "Mensagem" // nome antigo da coluna
          );
        }
    }
}
