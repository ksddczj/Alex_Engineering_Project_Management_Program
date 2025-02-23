using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alex_Engineering_Project_Management_Program.Migrations
{
    /// <inheritdoc />
    public partial class RenameNameToProjectName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "Name", table: "Projects", newName: "ProjectName");

            migrationBuilder.AlterColumn<string>(
                name: "StageName",
                table: "Stages",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "ProjectName", table: "Projects", newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "StageName",
                table: "Stages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT"
            );

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT"
            );
        }
    }
}
