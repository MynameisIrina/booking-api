using BookingApi.Application.Repositories;
using BookingApi.Application.UseCases.Bookings.Commands.GetById;
using BookingApi.Domain.Entities;
using Moq;

namespace BookingApi.Tests.ApplicationTests
{
    public class GetBookingByIdCommandHandlerTests
    {
        private readonly GetBookingByIdCommandHandler _handler;
        private Mock<IBookingRepository> _bookingRepositoryMock;

        public GetBookingByIdCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _handler = new GetBookingByIdCommandHandler(_bookingRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_BookingExists_ShouldReturnBooking()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var roomSlotId = Guid.NewGuid();
            var booking = new Booking(roomSlotId, "test@example.com");

            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId))
                .ReturnsAsync(booking);

            // Act
            var result = await _handler.Handle(new GetBookingByIdCommand(bookingId), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(booking.RoomSlotId, result.Value.RoomSlotId);
            Assert.Equal(booking.UserEmail, result.Value.UserEmail);
            Assert.Equal(booking.CreatedAt, result.Value.Date);
        }

        [Fact]
        public async Task Handle_BookingDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var bookingId = Guid.NewGuid();

            _bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await _handler.Handle(new GetBookingByIdCommand(bookingId), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
        }
    }
}