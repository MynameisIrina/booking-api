using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Persistence.Configurations
{
    public class RoomSlotConfiguration : IEntityTypeConfiguration<RoomSlot>
    {
        public void Configure(EntityTypeBuilder<RoomSlot> builder)
        {
            builder.Property(x => x.Xmin)
                .IsRowVersion()
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate();

        }
    }
}