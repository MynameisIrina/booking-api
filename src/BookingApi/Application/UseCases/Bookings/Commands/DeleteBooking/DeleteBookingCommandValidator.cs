using FluentValidation;

namespace BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
    {
        public DeleteBookingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Booking Id is required.");
        }
    }
}