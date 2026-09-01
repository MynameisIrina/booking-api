using FluentValidation;

namespace BookingApi.Application.UseCases.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.RoomSlotId)
                .NotEmpty().WithMessage("RoomSlotId is required.");

            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("User email is required.")
                .EmailAddress().WithMessage("User email must be a valid email address.");
        }
    }
}