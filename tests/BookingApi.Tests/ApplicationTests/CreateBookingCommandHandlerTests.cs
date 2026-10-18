using Ardalis.Result;
using BookingApi.Application.Repositories;
using BookingApi.Application.UseCases.Bookings.Commands.CreateBooking;
using BookingApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookingApi.Tests.ApplicationTests
{
    public class CreateBookingCommandHandlerTests
    {
        private Mock<IBookingRepository> bookingRepositoryMock;
        private Mock<IRoomSlotRepository> roomSlotRepositoryMock;
        private CreateBookingCommandHandler handler;

        public CreateBookingCommandHandlerTests()
        {
            bookingRepositoryMock = new Mock<IBookingRepository>();
            roomSlotRepositoryMock = new Mock<IRoomSlotRepository>();
            handler = new CreateBookingCommandHandler(bookingRepositoryMock.Object, roomSlotRepositoryMock.Object, Mock.Of<ILogger<CreateBookingCommandHandler>>(), TimeProvider.System);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateBooking()
        {
            // Arrange
            var roomSlotId = Guid.NewGuid();
            var userEmail = "test@example.com";

            var roomSlot = new RoomSlot("Conference Room A", DateTime.UtcNow.AddDays(1));

            roomSlotRepositoryMock.Setup(r => r.GetRoomSlotByIdTrackedAsync(roomSlotId))
                .ReturnsAsync(roomSlot);

            Booking? capturedBooking = null;
            bookingRepositoryMock.Setup(r => r.CreateBookingAsync(It.IsAny<Booking>()))
                .Callback<Booking>(b => capturedBooking = b)
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(new CreateBookingCommand(roomSlotId, userEmail), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(capturedBooking);
            Assert.Equal(capturedBooking.Id, result.Value);
        }

        [Fact]
        public async Task Handle_RoomSlotNotFound_ShouldReturnNotFoundResult()
        {
            // Arrange
            var roomSlotId = Guid.NewGuid();
            var userEmail = "test@example.com";

            roomSlotRepositoryMock.Setup(r => r.GetRoomSlotByIdTrackedAsync(roomSlotId))
                .ReturnsAsync((RoomSlot?)null);

            // Act
            var result = await handler.Handle(new CreateBookingCommand(roomSlotId, userEmail), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.NotFound, result.Status);
        }

        [Fact]
        public async Task Handle_RoomSlotAlreadyBooked_ShouldReturnConflictResult()
        {
            // Arrange
            var roomSlotId = Guid.NewGuid();
            var userEmail = "test@example.com";

            var roomSlot = new RoomSlot("Conference Room A", DateTime.UtcNow.AddDays(1));

            roomSlotRepositoryMock.Setup(r => r.GetRoomSlotByIdTrackedAsync(roomSlotId))
                .ReturnsAsync(roomSlot);

            Booking? capturedBooking = null;
            bookingRepositoryMock.Setup(r => r.CreateBookingAsync(It.IsAny<Booking>()))
                .Callback<Booking>(b => capturedBooking = b)
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(new CreateBookingCommand(roomSlotId, userEmail), CancellationToken.None);

            // Act again to simulate a second booking attempt
            var secondResult = await handler.Handle(new CreateBookingCommand(roomSlotId, userEmail), CancellationToken.None);

            // Assert
            Assert.False(secondResult.IsSuccess);
            Assert.Equal(ResultStatus.Conflict, secondResult.Status);
        }

        [Fact]
        public async Task Handle_RepositoryNotCalledOnConflict_ShouldNotCreateBooking()
        {
            // Arrange
            var roomSlotId = Guid.NewGuid();
            var userEmail = "test@example.com";

            var roomSlot = new RoomSlot("Conference Room A", DateTime.UtcNow.AddDays(1));

            roomSlotRepositoryMock.Setup(r => r.GetRoomSlotByIdTrackedAsync(roomSlotId))
                .ReturnsAsync(roomSlot);

            // Act
            var result = await handler.Handle(new CreateBookingCommand(roomSlotId, userEmail), CancellationToken.None);

            // Act again to simulate a second booking attempt
            var secondResult = await handler.Handle(new CreateBookingCommand(roomSlotId, userEmail), CancellationToken.None);

            // Assert
            Assert.False(secondResult.IsSuccess);
            Assert.Equal(ResultStatus.Conflict, secondResult.Status);
            bookingRepositoryMock.Verify(r => r.CreateBookingAsync(It.IsAny<Booking>()), Times.Once);
        }
    }
}