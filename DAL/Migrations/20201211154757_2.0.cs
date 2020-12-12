using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeNumber2.Migrations
{
    public partial class _20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Numbers");

            migrationBuilder.CreateTable(
                name: "PrimeNumbers",
                columns: table => new
                {
                    NTH = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDN = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimeNumbers", x => x.NTH);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrimeNumbers");

            migrationBuilder.CreateTable(
                name: "Numbers",
                columns: table => new
                {
                    Pos = table.Column<Guid>(type: "Guid", nullable: false),
                    IDN = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Numbers", x => x.Pos);
                });
        }
    }
}
