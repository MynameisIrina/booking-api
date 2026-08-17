using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using BookingApi.Application.Interfaces.Abstractions;

namespace BookingApi.Application.UseCases.Bookings.Commands.GetById
{
    public sealed record GetBookingByIdCommand(Guid Id): ICommand<Result<BookingDto>>;
    
    public sealed record BookingDto(Guid RoomSlotId, string UserEmail, DateTime Date);
}