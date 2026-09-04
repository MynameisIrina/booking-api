using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Domain.Entities;

namespace BookingApi.Tests.DomainTests
{
    public class RoomSlotsTests
    {

        [Fact]
        public void RoomSlot_CreateWithValidParameters_ShouldCreateRoomSlot()
        {
            // Arrange
            var roomName = "Conference Room A";
            var slotDate = DateTime.UtcNow.AddDays(1);

            // Act
            var roomSlot = new RoomSlot(roomName, slotDate);

            // Assert
            Assert.Equal(roomName, roomSlot.RoomName);
            Assert.Equal(slotDate, roomSlot.SlotDate);
        }

        [Fact]
        public void RoomSlot_CreateWithEmptyRoomName_ShouldThrowArgumentException()
        {
            // Arrange
            var roomName = "";
            var slotDate = DateTime.UtcNow.AddDays(1);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new RoomSlot(roomName, slotDate));
            Assert.Equal("Room name cannot be null or empty. (Parameter 'roomName')", exception.Message);
        }

        [Fact]
        public void RoomSlot_BookNotBookedRoom_ShouldBookRoomSlot()
        {
            // Arrange
            var roomSlot = new RoomSlot("Conference Room A", DateTime.UtcNow.AddDays(1));
            var userEmail = "test.@example.com";
            roomSlot.GetType().GetProperty("IsBooked")!.SetValue(roomSlot, false);

            // Act
            var result = roomSlot.Book(userEmail);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(roomSlot.IsBooked);
        }

        [Fact]
        public void RoomSlot_BookAlreadyBookedRoom_ShouldReturnInvalidResult()
        {
            // Arrange
            var roomSlot = new RoomSlot("Conference Room A", DateTime.UtcNow.AddDays(1));
            var userEmail = "test@example.com";
            roomSlot.GetType().GetProperty("IsBooked")!.SetValue(roomSlot, true);

            // Act
            var result = roomSlot.Book(userEmail);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"Room slot {roomSlot.Id} is already booked.", result.ValidationErrors.First().ErrorMessage);
        }
    }
}