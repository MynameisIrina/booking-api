using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApi.Domain.Interfaces
{
    public abstract class DomainEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public uint Xmin { get; set; }
    }
}