using Ardalis.Result;
using BookingApi.Application.Abstractions;
using BookingApi.Application.Repositories;
using BookingApi.Application.UseCases.Bookings.Queries.GetById;
using BookingApi.Domain.Entities;
using Moq;

namespace BookingApi.Tests.ApplicationTests;

public class GetBookingByIdQueryHandlerTests
{
    private readonly GetBookingByIdQueryHandler _handler;
    private Mock<IBookingRepository> _bookingRepositoryMock;

    public GetBookingByIdQueryHandlerTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _handler = new GetBookingByIdQueryHandler(_bookingRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_BookingExists_ShouldReturnBooking()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var roomSlotId = Guid.NewGuid();
        var booking = new Booking(roomSlotId, "test@example.com", DateTimeOffset.UtcNow);

        _bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId))
            .ReturnsAsync(booking);

        // Act
        var result = await _handler.Handle(new GetBookingByIdQuery(bookingId), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(booking.RoomSlotId, result.Value.RoomSlotId);
        Assert.Equal(booking.UserEmail, result.Value.UserEmail);
        Assert.Equal(booking.CreatedAt, result.Value.CreatedAt);
    }

    [Fact]
    public async Task Handle_BookingDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var bookingId = Guid.NewGuid();

        _bookingRepositoryMock.Setup(r => r.GetByIdAsync(bookingId))
            .ReturnsAsync((Booking?)null);

        // Act
        var result = await _handler.Handle(new GetBookingByIdQuery(bookingId), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public void GetBookingById_IsQueryNotCommand()
    {
        var booking = new GetBookingByIdQuery(Guid.Empty);
        Assert.False(booking is ICommand<Result<BookingDto>>);
    }
}
