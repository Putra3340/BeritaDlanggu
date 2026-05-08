using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeritaDlanggu.Migrations
{
    /// <inheritdoc />
    public partial class DynamicNavRev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "NavSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NavSettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "NavSettings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NavSettings_ParentId",
                table: "NavSettings",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NavSettings_NavSettings",
                table: "NavSettings",
                column: "ParentId",
                principalTable: "NavSettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavSettings_NavSettings",
                table: "NavSettings");

            migrationBuilder.DropIndex(
                name: "IX_NavSettings_ParentId",
                table: "NavSettings");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "NavSettings");

            migrationBuilder.AlterColumn<int>(
                name: "Title",
                table: "NavSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "NavSettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
