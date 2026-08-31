using Ardalis.Result;
using BookingApi.Domain.Interfaces;

namespace BookingApi.Domain.Entities;

public class RoomSlot : DomainEntity
{
    public string RoomName { get; private set; } = string.Empty;
    public DateTime SlotDate { get; private set; }
    public bool IsBooked { get; private set; }
    public string? BookedByEmail { get; private set; }

    private RoomSlot() { }
    public RoomSlot(string roomName, DateTime slotDate)
    {
        RoomName = roomName;
        SlotDate = slotDate;
    }

    public Result Book(string userEmail)
    {
        if(string.IsNullOrWhiteSpace(userEmail))
        {
            throw new ArgumentException("User email cannot be null or empty.", nameof(userEmail));
        }

        if (IsBooked)
        {
            return Result.Invalid(new ValidationError($"Room slot {Id} is already booked."));
        }

        IsBooked = true;
        BookedByEmail = userEmail;
        return Result.Success();
    }
}