using BookingApi.Domain.Interfaces;

namespace BookingApi.Domain.Entities;

public class RoomSlot : DomainEntity
{
    public string RoomName { get; private set; } = string.Empty;
    public DateTime SlotDate { get; private set; }
    public bool IsBooked { get; set; }
    public string? BookedByEmail { get; set; }

    private RoomSlot() { }
    public RoomSlot(string roomName, DateTime slotDate)
    {
        RoomName = roomName;
        SlotDate = slotDate;
    }

    public void Book(string userEmail)
    {
        if (IsBooked)
        {
            throw new InvalidOperationException("Room slot is already booked.");
        }

        IsBooked = true;
        BookedByEmail = userEmail;
    }
}