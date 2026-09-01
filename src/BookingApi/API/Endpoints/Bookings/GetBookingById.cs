using BookingApi.API.Extensions;
using BookingApi.Application.UseCases.Bookings.Commands.GetById;
using FastEndpoints;
using MediatR;

namespace BookingApi.API.Endpoints.Bookings
{
    public class GetBookingById(IMediator mediator) : Endpoint<GetBookingRequest, GetBookingResponse>
    {
        public override void Configure()
        {
            Get("/bookings/{Id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetBookingRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new GetBookingByIdCommand(request.Id), ct);

            if (!result.IsSuccess)
            {
                foreach (var error in result.ValidationErrors)
                {
                    AddError(error.ErrorMessage);
                }
                await Send.ErrorsAsync(result.Status.ToHttpStatusCode(), cancellation: ct);
                return;
            }

            var response = new GetBookingResponse(result.Value);
            await Send.OkAsync(response, cancellation: ct);
        }
    }

    public sealed record GetBookingRequest
    {
        public Guid Id { get; init; }
    }

    public sealed record GetBookingResponse(BookingDto booking);
}