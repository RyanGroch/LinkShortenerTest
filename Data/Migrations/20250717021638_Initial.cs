using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmallUrl.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortLink",
                columns: table => new
                {
                    ShortLinkId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Destination = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortLink", x => x.ShortLinkId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortLink_Slug",
                table: "ShortLink",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortLink");
        }
    }
}
