using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PrimeNumber2.Migrations
{
    public partial class _13_alignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pos",
                table: "Numbers"
                );

            migrationBuilder.AlterColumn<Guid>(
                name: "Position",
                table: "Numbers",
                oldType: "int",
                type: "uniqueidentifier"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pos",
                table: "Numbers",
                newName: "Position");
        }
    }
}
