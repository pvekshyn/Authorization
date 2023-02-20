using Grpc.Core;
using Quartz;

namespace Inbox.Job.Infrastructure
{
    [DisallowConcurrentExecution]
    internal class InboxJob : IJob
    {
        private readonly IInboxRepository _inboxRepository;
        private readonly GrpcEventProcessingService.GrpcEventProcessingServiceClient _grpcEventProcessingClient;
        public InboxJob(IInboxRepository inboxRepository, GrpcEventProcessingService.GrpcEventProcessingServiceClient grpcEventProcessingClient)
        {
            _inboxRepository = inboxRepository;
            _grpcEventProcessingClient = grpcEventProcessingClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var inboxMessage = _inboxRepository.GetFirst();

            while (inboxMessage != null)
            {
                try
                {
                    var request = new GrpcEventRequest { Event = inboxMessage.Message };

                    var result = await _grpcEventProcessingClient.ProcessEventAsync(request);

                    if (!result.IsSuccess)
                    {
                        if (_inboxRepository.ErrorsAny(inboxMessage.Id))
                        {
                            break;
                        }

                        _inboxRepository.InsertError(inboxMessage.Id, result.ErrorMessage, result.StackTrace);
                        break;
                    }

                    _inboxRepository.Delete(inboxMessage.Id);
                }
                catch (RpcException e)
                {
                    if (_inboxRepository.ErrorsAny(inboxMessage.Id))
                    {
                        break;
                    }

                    _inboxRepository.InsertError(inboxMessage.Id, e);
                    break;
                }
                inboxMessage = _inboxRepository.GetFirst();
            }
        }
    }
}