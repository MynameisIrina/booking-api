using BookingApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Persistence.Configurations
{
    public class DomainEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : DomainEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Xmin)
                .IsRowVersion()
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}