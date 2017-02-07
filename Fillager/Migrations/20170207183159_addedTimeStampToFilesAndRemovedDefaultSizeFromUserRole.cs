using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fillager.Migrations
{
    public partial class addedTimeStampToFilesAndRemovedDefaultSizeFromUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultStorage",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "File",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "File");

            migrationBuilder.AddColumn<long>(
                name: "DefaultStorage",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
