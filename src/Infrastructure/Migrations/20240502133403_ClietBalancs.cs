using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AML.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClietBalancs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "CustomerBalance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    Currency = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerBalance_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBalance_CustomerId",
                table: "CustomerBalance",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerBalance");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
