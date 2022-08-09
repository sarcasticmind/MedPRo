using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace keef2.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "offerDate",
                table: "Doctors",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "offerDate",
                table: "Doctors");
        }
    }
}
