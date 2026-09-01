using Ardalis.Result;

namespace BookingApi.Domain.Entities
{
    public class Booking : DomainEntity
    {
        public Guid RoomSlotId { get; private set; }
        public string UserEmail { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Booking() { }
        public Booking(Guid roomSlotId, string userEmail)
        {
            RoomSlotId = roomSlotId;
            UserEmail = userEmail;
        }

        public Result EnsureCanBeDeleted()
        {
            if (CreatedAt.AddMinutes(60) < DateTime.UtcNow)
            {
                return Result.Invalid(new ValidationError("Booking can be deleted only within 60 minutes of creation."));
            }

            return Result.Success();
        }

    }
}