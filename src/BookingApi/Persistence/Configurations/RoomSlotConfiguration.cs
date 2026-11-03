using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Persistence.Configurations;

public class RoomSlotConfiguration : DomainEntityConfiguration<RoomSlot>
{
    public override void Configure(EntityTypeBuilder<RoomSlot> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => new { x.RoomName, x.SlotDate })
            .IsUnique();

        builder.HasIndex(x => new { x.SlotDate, x.RoomName })
            .HasFilter("NOT \"IsBooked\"");
    }
}