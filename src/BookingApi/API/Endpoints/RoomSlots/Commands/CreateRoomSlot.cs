using BookingApi.API.Extensions;
using BookingApi.Application.UseCases.RoomSlots.Commands;
using FastEndpoints;
using MediatR;

namespace BookingApi.API.Endpoints.RoomSlots.Commands
{
    public class CreateRoomSlot(IMediator mediator) : Endpoint<CreateRoomSlotRequest, CreateRoomSlotResponse>
    {
        public override void Configure()
        {
            Post("/room-slots");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateRoomSlotRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateRoomSlotCommand(req.RoomName, req.SlotDate, req.UserEmail), ct);
            if (!result.IsSuccess)
            {
                foreach (var error in result.ValidationErrors)
                {
                    AddError(error.ErrorMessage);
                }
                await Send.ErrorsAsync(result.Status.ToHttpStatusCode(), ct);
                return;
            }

            var response = new CreateRoomSlotResponse(result.Value);
            await Send.OkAsync(response, ct);
        }

    }

    public sealed record CreateRoomSlotRequest(
        string RoomName,
        DateTime SlotDate,
        string UserEmail
    );

    public sealed record CreateRoomSlotResponse(Guid RoomSlotId);
}