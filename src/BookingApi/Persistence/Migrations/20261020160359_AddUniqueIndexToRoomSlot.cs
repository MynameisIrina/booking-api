using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToRoomSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RoomSlots_RoomName_SlotDate",
                table: "RoomSlots",
                columns: new[] { "RoomName", "SlotDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomSlots_RoomName_SlotDate",
                table: "RoomSlots");
        }
    }
}
