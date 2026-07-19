using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Domain.Interfaces;

namespace BookingApi.Domain.Entities
{
    public class Booking : DomainEntity
    {
        public Guid RoomSlotId { get; private set; }
        public string UserEmail { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Booking() { }
        public Booking(Guid roomSlotId, string userEmail)
        {
            RoomSlotId = roomSlotId;
            UserEmail = userEmail;
        }
    }
}