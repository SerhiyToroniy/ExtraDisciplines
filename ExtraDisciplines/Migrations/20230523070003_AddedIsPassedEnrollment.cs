using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDisciplines.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsPassedEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPassed",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPassed",
                table: "Enrollments");
        }
    }
}
