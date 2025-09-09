using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stores.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackColumnInApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "StoreAccountApplications",
                type: "Text",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "StoreAccountApplications");
        }
    }
}
