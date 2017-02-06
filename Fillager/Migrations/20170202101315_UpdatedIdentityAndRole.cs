using Microsoft.EntityFrameworkCore.Migrations;

namespace Fillager.Migrations
{
    public partial class UpdatedIdentityAndRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                "EarnedExtraStorage",
                "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "OtherStorageBonus",
                "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "PayedExtraStorage",
                "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "StorageUsed",
                "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                "DefaultStorage",
                "AspNetRoles",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "DefaultStorage",
                "AspNetRoles");

            migrationBuilder.DropColumn(
                "EarnedExtraStorage",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "OtherStorageBonus",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "PayedExtraStorage",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "StorageUsed",
                "AspNetUsers");
        }
    }
}