using BookingApi.API.Extensions;
using BookingApi.Application.UseCases.Bookings.Commands.CreateBooking;
using FastEndpoints;
using MediatR;

namespace BookingApi.API.Endpoints.Bookings
{
    public class CreateBooking(IMediator mediator) : Endpoint<CreateBookingRequest, CreateBookingResponse>
    {
        public override void Configure()
        {
            Post("/bookings/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateBookingRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateBookingCommand(request.RoomSlotId, request.UserEmail), ct);
            if (!result.IsSuccess)
            {
                foreach (var error in result.ValidationErrors)
                {
                    AddError(error.ErrorMessage);
                }
                await Send.ErrorsAsync(result.Status.ToHttpStatusCode(), cancellation: ct);
                return;
            }

            var response = new CreateBookingResponse(result.Value);

            await Send.CreatedAtAsync<CreateBooking>(request, response, cancellation: ct);
        }
    }

    public sealed record CreateBookingRequest(Guid RoomSlotId, string UserEmail);

    public sealed record CreateBookingResponse(Guid BookingId);
}