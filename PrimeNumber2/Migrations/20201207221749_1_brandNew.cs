using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrimeNumber2.Migrations
{
    public partial class _1_brandNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Numbers",
                columns: table => new
                {
                    Position = table.Column<Guid>(type: "int primary key identity", nullable: false),
                    IDN = table.Column<long>(type: "bigint", nullable: false)
                });
                //constraints: table =>
                //{
                //    table.PrimaryKey("PK_Numbers", x => x.Position);
                //});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Numbers");
        }
    }
}
