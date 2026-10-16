using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Application.Repositories;
using BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking;
using BookingApi.Domain.Entities;
using Moq;

namespace BookingApi.Tests.ApplicationTests
{
    public class DeleteBookingCommandHandlerTests
    {
        private readonly DeleteBookingCommandHandler handler;
        private Mock<IBookingRepository> bookingRepositoryMock;
        private Mock<IRoomSlotRepository> roomSlotRepositoryMock;

        public DeleteBookingCommandHandlerTests()
        {
            bookingRepositoryMock = new Mock<IBookingRepository>();
            roomSlotRepositoryMock = new Mock<IRoomSlotRepository>();
            handler = new DeleteBookingCommandHandler(bookingRepositoryMock.Object, roomSlotRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_BookingExistsAndCanBeDeleted_ShouldDeleteBooking()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var roomSlotId = Guid.NewGuid();
            var roomSlot = new RoomSlot("TestRoom", DateTime.UtcNow);
            var booking = new Booking(roomSlotId, "test@example.com");
            roomSlot.GetType().GetProperty("IsBooked")?.SetValue(roomSlot, true);

            bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId))
                .ReturnsAsync(booking); 
            
            roomSlotRepositoryMock.Setup(r => r.GetRoomSlotByIdTrackedAsync(roomSlotId))
                .ReturnsAsync(roomSlot);
            
            bookingRepositoryMock.Setup(r => r.Delete(booking))
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(new DeleteBookingCommand(bookingId), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            bookingRepositoryMock.Verify(r => r.Delete(booking), Times.Once);
        }

        [Fact]
        public async Task Handle_BookingDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var bookingId = Guid.NewGuid();

            bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await handler.Handle(new DeleteBookingCommand(bookingId), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
        }

        [Fact]
        public async Task Handle_BookingCannotBeDeleted_ShouldReturnFailure()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var roomSlotId = Guid.NewGuid();
            var booking = new Booking(roomSlotId, "test@example.com");
            var roomSlot = new RoomSlot("TestRoom", DateTime.UtcNow);

            // Simulate a booking that cannot be deleted
            booking.GetType().GetProperty("CreatedAt")?.SetValue(booking, DateTime.UtcNow.AddMinutes(-120));

            bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId))
                .ReturnsAsync(booking);

            bookingRepositoryMock.Setup(r => r.Delete(booking))
                .Returns(Task.CompletedTask);

            roomSlotRepositoryMock.Setup(r => r.GetRoomSlotByIdTrackedAsync(roomSlotId))
                .ReturnsAsync(roomSlot);

            // Act
            var result = await handler.Handle(new DeleteBookingCommand(bookingId), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Ardalis.Result.ResultStatus.Invalid, result.Status);
        }
    }
}