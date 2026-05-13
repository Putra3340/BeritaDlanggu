using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeritaDlanggu.Migrations
{
    /// <inheritdoc />
    public partial class DynamicNavRevRev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "NavSettings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "NavSettings");
        }
    }
}
