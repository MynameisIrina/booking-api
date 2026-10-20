using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFKToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomSlotId",
                table: "Bookings",
                column: "RoomSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_RoomSlots_RoomSlotId",
                table: "Bookings",
                column: "RoomSlotId",
                principalTable: "RoomSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_RoomSlots_RoomSlotId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomSlotId",
                table: "Bookings");
        }
    }
}
