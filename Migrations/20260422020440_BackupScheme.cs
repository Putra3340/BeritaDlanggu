using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeritaDlanggu.Migrations
{
    /// <inheritdoc />
    public partial class BackupScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            migrationBuilder.Sql("EXEC sp_MSforeachtable 'DELETE FROM ?'");
            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");
            migrationBuilder.Sql(@"
DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql += 
'DBCC CHECKIDENT ([' + t.name + '], RESEED, 0);'
FROM sys.tables t
INNER JOIN sys.identity_columns ic ON t.object_id = ic.object_id;

EXEC sp_executesql @sql;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Articles",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
