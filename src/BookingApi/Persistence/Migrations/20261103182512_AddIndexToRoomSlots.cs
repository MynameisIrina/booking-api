using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToRoomSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RoomSlots_SlotDate_RoomName",
                table: "RoomSlots",
                columns: new[] { "SlotDate", "RoomName" },
                filter: "NOT \"IsBooked\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomSlots_SlotDate_RoomName",
                table: "RoomSlots");
        }
    }
}
