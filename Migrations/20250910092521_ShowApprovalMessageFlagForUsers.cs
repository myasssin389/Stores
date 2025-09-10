using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stores.Migrations
{
    /// <inheritdoc />
    public partial class ShowApprovalMessageFlagForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowStoreAccountApprovalMessage",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowStoreAccountApprovalMessage",
                table: "AspNetUsers");
        }
    }
}
