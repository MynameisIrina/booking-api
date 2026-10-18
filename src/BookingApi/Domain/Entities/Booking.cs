using Ardalis.Result;

namespace BookingApi.Domain.Entities
{
    public class Booking : DomainEntity
    {
        public Guid RoomSlotId { get; private set; }
        public string UserEmail { get; private set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; private set; }

        private Booking() { }
        public Booking(Guid roomSlotId, string userEmail, DateTimeOffset now)
        {
            if (roomSlotId == Guid.Empty)
                throw new ArgumentException("RoomSlotId cannot be empty.", nameof(roomSlotId));

            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("UserEmail cannot be empty.", nameof(userEmail));

            RoomSlotId = roomSlotId;
            UserEmail = userEmail;
            CreatedAt = now.ToUniversalTime();
        }

        public Result EnsureCanBeDeleted(DateTimeOffset now)
        {
            if (CreatedAt.AddMinutes(60) <= now)
            {
                return Result.Invalid(new ValidationError("Booking can be deleted only within 60 minutes of creation."));
            }

            return Result.Success();
        }

    }
}