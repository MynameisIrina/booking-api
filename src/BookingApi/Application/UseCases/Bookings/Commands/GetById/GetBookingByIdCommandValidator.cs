using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace BookingApi.Application.UseCases.Bookings.Commands.GetById
{
    public class GetBookingByIdCommandValidator: AbstractValidator<GetBookingByIdCommand>
    {
        public GetBookingByIdCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(id => id != Guid.Empty).WithMessage("Id cannot be an empty GUID.");
        }
        
    }
}