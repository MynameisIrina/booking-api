using BookingApi.Application.UseCases.Bookings.CreateBooking;
using FastEndpoints;
using MediatR;

namespace BookingApi.API.Endpoints.Bookings
{
    public class CreateBooking(IMediator mediator): Endpoint<CreateBookingRequest, CreateBookingResponse>
    {
        public override void Configure()
        {
            Post("/bookings");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateBookingRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateBookingCommand(request.RoomSlotId, request.UserEmail), ct);
            if(!result.IsSuccess)
            {
                await Send.ErrorsAsync((int) result.Status, ct);
                return;
            }

            var response = new CreateBookingResponse(result.Value);

            await Send.CreatedAtAsync<CreateBooking>(request, response, cancellation: ct);
        }
    }

    public sealed record CreateBookingRequest(Guid RoomSlotId, string UserEmail);

    public sealed record CreateBookingResponse(Guid BookingId);
}