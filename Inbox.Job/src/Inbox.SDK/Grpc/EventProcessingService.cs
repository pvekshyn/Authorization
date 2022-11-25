using Common.SDK;
using Grpc.Core;
using Inbox.SDK.EventToRequest;
using MediatR;
using Newtonsoft.Json;

namespace Inbox.SDK.Grpc;

public class EventProcessingService : GrpcEventProcessingService.GrpcEventProcessingServiceBase
{
    private readonly IRequestFactory _requestFactory;
    private readonly IMediator _mediator;
    public EventProcessingService(IRequestFactory requestFactory, IMediator mediator)
    {
        _requestFactory = requestFactory;
        _mediator = mediator;
    }

    public override async Task<GrpcResult> ProcessEvent(GrpcEventRequest request, ServerCallContext context)
    {
        try
        {
            var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

            var pubSubEvent = JsonConvert.DeserializeObject(request.Event, jsonSerializerSettings);

            var mediatorRequest = _requestFactory.GetRequest((IEvent)pubSubEvent);

            var response = await _mediator.Send(mediatorRequest);
            if (!response.IsSuccess)
            {
                return new GrpcResult
                {
                    IsSuccess = false,
                    ErrorMessage = response.Errors.First().Code
                };
            }

            return new GrpcResult { IsSuccess = true };
        }
        catch (Exception e)
        {
            return new GrpcResult
            {
                IsSuccess = false,
                ErrorMessage = e.Message,
                StackTrace = e.StackTrace
            };
        }
    }
}
