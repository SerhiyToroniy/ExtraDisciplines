using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDisciplines.Migrations
{
    /// <inheritdoc />
    public partial class AddedAlreadyEnrolledCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlreadyEnrolledCount",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlreadyEnrolledCount",
                table: "Courses");
        }
    }
}
