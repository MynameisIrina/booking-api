using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using BookingApi.Domain.Interfaces;

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

        public Result DeleteBooking()
        {
            if(CreatedAt.AddHours(24) < DateTime.UtcNow)
            {
                return Result.Invalid(new ValidationError("Booking cannot be deleted after 24 hours of creation."));
            }
            
            return Result.Success();
        }

    }
}