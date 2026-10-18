using BookingApi.Domain.Entities;
using Microsoft.Extensions.Time.Testing;

namespace BookingApi.Tests.DomainTests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_CreateWithValidParameters_ShouldCreateBooking()
        {
            // Arrange
            var roomSlotId = Guid.CreateVersion7();
            var userEmail = "test@example.com";

            // Act
            var booking = new Booking(roomSlotId, userEmail, DateTimeOffset.UtcNow);

            // Assert
            Assert.Equal(roomSlotId, booking.RoomSlotId);
            Assert.Equal(userEmail, booking.UserEmail);
        }

        [Fact]
        public void Booking_CreateWithEmptyRoomSlotId_ShouldThrowArgumentException()
        {
            // Arrange
            var roomSlotId = Guid.Empty;
            var userEmail = "test@example.com";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Booking(roomSlotId, userEmail, DateTimeOffset.UtcNow));
            Assert.Equal("RoomSlotId cannot be empty. (Parameter 'roomSlotId')", exception.Message);
        }

        [Fact]
        public void Booking_CreateWithEmptyUserEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var roomSlotId = Guid.CreateVersion7();
            var userEmail = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Booking(roomSlotId, userEmail, DateTimeOffset.UtcNow));
            Assert.Equal("UserEmail cannot be empty. (Parameter 'userEmail')", exception.Message);
        }

        [Fact]
        public void Booking_EnsureCanBeDeleted_Within60Minutes_ShouldReturnSuccess()
        {
            // Arrange
            var roomSlotId = Guid.CreateVersion7();
            var userEmail = "test@example.com";

            // Act
            var booking = new Booking(roomSlotId, userEmail, DateTimeOffset.UtcNow);
            var result = booking.EnsureCanBeDeleted(booking.CreatedAt.AddMinutes(5));

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Booking_EnsureCanBeDeleted_After60Minutes_ShouldReturnInvalid()
        {
            // Arrange
            var roomSlotId = Guid.CreateVersion7();
            var userEmail = "test@example.com";

            // Act
            var booking = new Booking(roomSlotId, userEmail, DateTimeOffset.UtcNow);
            var result = booking.EnsureCanBeDeleted(booking.CreatedAt.AddMinutes(61));

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Booking can be deleted only within 60 minutes of creation.", result.ValidationErrors.First().ErrorMessage);
        }

        [Fact]
        public void Booking_EnsureCanBeDeleted_Exactly60Minutes_ShouldReturnInvalid()
        {
            // Arrange
            var roomSlotId = Guid.CreateVersion7();
            var userEmail = "test@example.com";

            // Act
            var booking = new Booking(roomSlotId, userEmail, DateTimeOffset.UtcNow);
            var result = booking.EnsureCanBeDeleted(booking.CreatedAt.AddMinutes(60));

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Booking can be deleted only within 60 minutes of creation.", result.ValidationErrors.First().ErrorMessage);
        }
    }
}