using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stores.Migrations
{
    /// <inheritdoc />
    public partial class LinkingVerStatusToStores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "VerificationStatusId",
                table: "Stores",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_VerificationStatusId",
                table: "Stores",
                column: "VerificationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_VerificationStatuses_VerificationStatusId",
                table: "Stores",
                column: "VerificationStatusId",
                principalTable: "VerificationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_VerificationStatuses_VerificationStatusId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_VerificationStatusId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "VerificationStatusId",
                table: "Stores");
        }
    }
}
