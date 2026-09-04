using Ardalis.Result;
using BookingApi.Application.Abstractions;

namespace BookingApi.Application.UseCases.RoomSlots.Commands
{
    public sealed record CreateRoomSlotCommand(string RoomName, DateTime SlotDate, string UserEmail) : ICommand<Result<Guid>>;
}