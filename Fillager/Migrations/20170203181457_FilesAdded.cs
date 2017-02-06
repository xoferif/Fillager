using Microsoft.EntityFrameworkCore.Migrations;

namespace Fillager.Migrations
{
    public partial class FilesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "File",
                table => new
                {
                    FileId = table.Column<string>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    OwnerGuidId = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                    table.ForeignKey(
                        "FK_File_AspNetUsers_OwnerGuidId",
                        x => x.OwnerGuidId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_File_OwnerGuidId",
                "File",
                "OwnerGuidId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "File");
        }
    }
}