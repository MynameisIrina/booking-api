using FluentValidation;

namespace BookingApi.Application.UseCases.RoomSlots.Commands
{
    public class CreateRoomSlotValidator : AbstractValidator<CreateRoomSlotCommand>
    {
        public CreateRoomSlotValidator()
        {
            RuleFor(x => x.RoomName)
                .NotEmpty().WithMessage("Room name is required.")
                .MaximumLength(100).WithMessage("Room name must not exceed 100 characters.");

            RuleFor(x => x.SlotDate)
                .NotEmpty().WithMessage("Slot date is required.")
                .Must(date => date > DateTime.MinValue && date < DateTime.MaxValue).WithMessage("Slot date must be a valid date.");
        }

    }
}