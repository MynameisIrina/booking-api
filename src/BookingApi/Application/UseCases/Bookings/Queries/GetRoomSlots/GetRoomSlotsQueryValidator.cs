using FluentValidation;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetRoomSlots
{
    public class GetRoomSlotsQueryValidator : AbstractValidator<GetRoomSlotsQuery>
    {
        public GetRoomSlotsQueryValidator()
        {
            RuleFor(x => x.PagedRequest.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PagedRequest.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }

    }
}