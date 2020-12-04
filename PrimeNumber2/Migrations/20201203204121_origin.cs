using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeNumber2.Migrations
{
    public partial class origin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrimeNumbers",
                columns: table => new
                {
                    IDN = table.Column<decimal>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimeNumbers", x => x.IDN);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrimeNumbers");
        }
    }
}
