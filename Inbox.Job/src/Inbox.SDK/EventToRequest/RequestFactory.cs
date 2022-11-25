using Common.SDK;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Inbox.SDK.EventToRequest
{
    public interface IRequestFactory
    {
        IRequest<Result> GetRequest(IEvent pubSubEvent);
    }

    internal class RequestFactory : IRequestFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RequestFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRequest<Result> GetRequest(IEvent pubSubEvent)
        {
            var mapperType = typeof(IEventToRequestMapper<>).MakeGenericType(pubSubEvent.GetType());

            var eventToRequestMapper = (IEventToRequestMapper)_serviceProvider.GetRequiredService(mapperType);
            return eventToRequestMapper.Map(pubSubEvent);
        }
    }
}
