using Ardalis.Result;
using BookingApi.Application.Abstractions;
using BookingApi.Application.Behaviors;
using BookingApi.Application.UseCases.Bookings.Commands.CreateBooking;
using BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace BookingApi.Tests.ApplicationTests;

public class SaveChangesBehaviourTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;

    public SaveChangesBehaviourTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Handle_Should_SaveChanges_When_Command_Is_Successful()
    {
        // Arrange
        var behavior = new SaveChangesBehavior<DeleteBookingCommand, Result>(_unitOfWorkMock.Object, NullLogger<SaveChangesBehavior<DeleteBookingCommand, Result>>.Instance);
        var id = Guid.NewGuid();
        var command = new DeleteBookingCommand(id);

        RequestHandlerDelegate<Result> next = _ => Task.FromResult(Result.Success());

        // Act
        var result = await behavior.Handle(command, next, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_SaveChanged_When_Command_Is_Successful_With_Result()
    {
        // Arrange
        var behavior = new SaveChangesBehavior<CreateBookingCommand, Result<Guid>>(_unitOfWorkMock.Object, NullLogger<SaveChangesBehavior<CreateBookingCommand, Result<Guid>>>.Instance);
        var id = Guid.NewGuid();
        var command = new CreateBookingCommand(Guid.NewGuid(), "test@example.com");

        RequestHandlerDelegate<Result<Guid>> next = _ => Task.FromResult(Result<Guid>.Success(id));

        // Act
        var result = await behavior.Handle(command, next, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Not_SaveChanges_When_Command_Is_Not_Successful_With_Result()
    {
        // Arrange
        var behavior = new SaveChangesBehavior<CreateBookingCommand, Result<Guid>>(_unitOfWorkMock.Object, NullLogger<SaveChangesBehavior<CreateBookingCommand, Result<Guid>>>.Instance);
        var command = new CreateBookingCommand(Guid.NewGuid(), "test@example.com");

        RequestHandlerDelegate<Result<Guid>> next = _ => Task.FromResult(Result<Guid>.Conflict("Failed to create booking"));

        // Act
        var result = await behavior.Handle(command, next, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Not_SaveChanges_When_Command_Is_Not_Successful()
    {
        // Arrange
        var behavior = new SaveChangesBehavior<DeleteBookingCommand, Result>(_unitOfWorkMock.Object, NullLogger<SaveChangesBehavior<DeleteBookingCommand, Result>>.Instance);
        var id = Guid.NewGuid();
        var command = new DeleteBookingCommand(id);

        RequestHandlerDelegate<Result> next = _ => Task.FromResult(Result.NotFound("Booking not found"));

        // Act
        var result = await behavior.Handle(command, next, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}