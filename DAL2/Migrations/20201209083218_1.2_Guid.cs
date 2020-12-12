using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PrimeNumber2.Migrations
{
    public partial class _12_Guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                type: "uniqueidentifier",
                name: "Pos",
                table: "Numbers",
                nullable: false
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Numbers"
                );
        }
    }
}
