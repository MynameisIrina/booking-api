using BookingApi.API.Extensions;
using BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking;
using FastEndpoints;
using MediatR;

namespace BookingApi.API.Endpoints.Bookings
{
    public class DeleteBooking(IMediator mediator) : Endpoint<DeleteBookingRequest, FastEndpoints.Void>
    {
        public override void Configure()
        {
            Delete("/bookings/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(DeleteBookingRequest req, CancellationToken ct)
        {
            var command = new DeleteBookingCommand(req.Id);
            var result = await mediator.Send(command, ct);

            if (!result.IsSuccess)
            {
                foreach(var error in result.ValidationErrors)
                {
                    AddError(error.ErrorMessage);
                }
                await Send.ErrorsAsync(result.Status.ToHttpStatusCode(), cancellation: ct);
                return;
            }

            await Send.NoContentAsync(cancellation: ct);
        }
    }

    public sealed record DeleteBookingRequest(Guid Id);
   
}