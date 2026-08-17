using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using BookingApi.Application.UseCases.Bookings.Commands.GetById;
using BookingApi.Domain.Entities;
using FastEndpoints;
using MediatR;

namespace BookingApi.API.Endpoints.Bookings
{
    public class GetBookingById(IMediator mediator): Endpoint<GetBookingRequest, GetBookingResponse>
    {
        public override void Configure()
        {
            Get("/bookings/{Id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetBookingRequest request, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetBookingByIdCommand(request.Id), cancellationToken);
            if(!result.IsSuccess)
            {
                await Send.ErrorsAsync((int) result.Status, cancellationToken);
                return;
                
            }

            var response = new GetBookingResponse(result.Value);
            await Send.OkAsync(response);
        }
    }

    public sealed record GetBookingRequest
    {
        public Guid Id { get; init; }
    }

    public sealed record GetBookingResponse(BookingDto booking);
}