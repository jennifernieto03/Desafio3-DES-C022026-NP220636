using Desafio03.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio03.Infrastructure.Persistence.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Clientes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                Apellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clientes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Productos",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Stock = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Productos", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Pedidos",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                Cantidad = table.Column<int>(type: "int", nullable: false),
                ClienteId = table.Column<int>(type: "int", nullable: false),
                ProductoId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Pedidos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Pedidos_Clientes_ClienteId",
                    column: x => x.ClienteId,
                    principalTable: "Clientes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Pedidos_Productos_ProductoId",
                    column: x => x.ProductoId,
                    principalTable: "Productos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Clientes_Email",
            table: "Clientes",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Pedidos_ClienteId",
            table: "Pedidos",
            column: "ClienteId");

        migrationBuilder.CreateIndex(
            name: "IX_Pedidos_ProductoId",
            table: "Pedidos",
            column: "ProductoId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Pedidos");
        migrationBuilder.DropTable(name: "Clientes");
        migrationBuilder.DropTable(name: "Productos");
    }
}
