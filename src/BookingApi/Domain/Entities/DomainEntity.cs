namespace BookingApi.Domain.Entities
{
    public abstract class DomainEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public uint Xmin { get; set; }
    }
}