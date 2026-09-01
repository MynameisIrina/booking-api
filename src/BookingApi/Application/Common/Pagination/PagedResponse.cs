namespace BookingApi.Application.Common.Pagination
{
    public class PagedResponse<T>
    {
        public IReadOnlyList<T> Data { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}