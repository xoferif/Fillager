using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fillager.Migrations
{
    public partial class UpdatedIdentityAndRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EarnedExtraStorage",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OtherStorageBonus",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PayedExtraStorage",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StorageUsedIn",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DefaultStorage",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultStorage",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "EarnedExtraStorage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OtherStorageBonus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PayedExtraStorage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StorageUsedIn",
                table: "AspNetUsers");
        }
    }
}
