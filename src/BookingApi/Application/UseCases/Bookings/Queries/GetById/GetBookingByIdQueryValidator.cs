using FluentValidation;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetById;

public class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
{
    public GetBookingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("Id cannot be an empty GUID.");
    }

}