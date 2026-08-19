using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Application.Common.Pagination;
using BookingApi.Application.UseCases.Bookings.Queries.GetRoomSlots;
using FastEndpoints;
using MediatR;

namespace BookingApi.API.Endpoints.RoomSlots
{
    public class GetRoomSlots(IMediator mediator): Endpoint<GetRoomSlotsRequest, GetRoomSlotsResponse>
    {
        public override void Configure()
        {
            Get("/room-slots/available");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetRoomSlotsRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRoomSlotsQuery(request), ct);
            if(!result.IsSuccess)
            {
                await Send.ErrorsAsync((int) result.Status, ct);
                return;
            }

            var response = new GetRoomSlotsResponse(result.Value);
            await Send.OkAsync(response, cancellation: ct);
        }
    }
    public sealed record GetRoomSlotsResponse(PagedResponse<RoomSlotDto> RoomSlots);

    public sealed record GetRoomSlotsRequest: PagedRequest;
}