using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Persistence.Configurations;

public class RoomSlotConfiguration : DomainEntityConfiguration<RoomSlot>
{
    public override void Configure(EntityTypeBuilder<RoomSlot> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => new {x.RoomName, x.SlotDate})
            .IsUnique();
    }
}