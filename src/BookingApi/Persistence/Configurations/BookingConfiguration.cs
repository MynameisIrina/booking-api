using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Persistence.Configurations;

public class BookingConfiguration : DomainEntityConfiguration<Booking>
{
    public override void Configure(EntityTypeBuilder<Booking> builder)
    {
        base.Configure(builder);

        builder.HasOne<RoomSlot>()
            .WithMany()
            .HasForeignKey(x => x.RoomSlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}