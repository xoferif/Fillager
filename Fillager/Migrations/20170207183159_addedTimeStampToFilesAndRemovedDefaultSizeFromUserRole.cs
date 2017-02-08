using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fillager.Migrations
{
    public partial class addedTimeStampToFilesAndRemovedDefaultSizeFromUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "DefaultStorage",
                "AspNetRoles");

            migrationBuilder.AddColumn<DateTime>(
                "CreatedDateTime",
                "File",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CreatedDateTime",
                "File");

            migrationBuilder.AddColumn<long>(
                "DefaultStorage",
                "AspNetRoles",
                nullable: false,
                defaultValue: 0L);
        }
    }
}