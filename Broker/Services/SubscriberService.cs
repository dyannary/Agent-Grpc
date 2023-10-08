using Broker.Models;
using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcsAgent;

namespace Broker.Services
{
    public class SubscriberService : Subscriber.SubscriberBase
    {
        private readonly IConnectionStorageService _connectionStorageService;

        public SubscriberService(IConnectionStorageService connectionStorageService)
        {
            _connectionStorageService = connectionStorageService;
        }

        public override Task<SubscribeReply> Subscribe(SubscribeRequest request, ServerCallContext context)
        {
            Console.WriteLine("New client trying to subscribe: " + request.Address, request.Topic);

            try
            {
                var connection = new SubscriberConnection(request.Address, request.Topic);

                _connectionStorageService.Add(connection);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not add a new connection " + request.Address + request.Topic + ": " + e.Message);
            }

            return Task.FromResult(new SubscribeReply()
            {
                IsSuccess = true
            });
        }
    }
}
