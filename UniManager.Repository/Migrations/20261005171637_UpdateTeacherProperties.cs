using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniManager.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeacherProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Teachers",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Students",
                newName: "lastName");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firstName",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Teachers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Students",
                newName: "Name");
        }
    }
}
