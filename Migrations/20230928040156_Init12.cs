using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.Migrations
{
    /// <inheritdoc />
    public partial class Init12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingCourts_BookingCartTypes_BookingCartTypeId",
                table: "BookingCourts");

            migrationBuilder.DropIndex(
                name: "IX_BookingCourts_BookingCartTypeId",
                table: "BookingCourts");

            migrationBuilder.DropColumn(
                name: "BookingCartTypeId",
                table: "BookingCourts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BookingCartTypeId",
                table: "BookingCourts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingCourts_BookingCartTypeId",
                table: "BookingCourts",
                column: "BookingCartTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingCourts_BookingCartTypes_BookingCartTypeId",
                table: "BookingCourts",
                column: "BookingCartTypeId",
                principalTable: "BookingCartTypes",
                principalColumn: "BookingCartTypeId");
        }
    }
}
