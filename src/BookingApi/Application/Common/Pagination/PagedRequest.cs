namespace BookingApi.Application.Common.Pagination
{
    public record PagedRequest
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;

    }
}