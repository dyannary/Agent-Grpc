using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcsAgent;

namespace Broker.Services
{
    public class SenderWorker : IHostedService
    {
        private Timer _timer;
        private const int TimeToWait = 2000;
        private readonly IContentStorageService _contentStorageService;
        private readonly IConnectionStorageService _connectionStorageService;

        public SenderWorker(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                _contentStorageService = scope.ServiceProvider.GetRequiredService<IContentStorageService>();
                _connectionStorageService = scope.ServiceProvider.GetRequiredService<IConnectionStorageService>();
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoSendWork, null, 0, TimeToWait);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoSendWork(object state)
        {
            while (!_contentStorageService.IsEmpty())
            {
                var content = _contentStorageService.GetNext();

                if (content != null)
                {
                    var connections = _connectionStorageService.GetConnectionsByTopic(content.Topic);

                    foreach (var connection in connections)
                    {
                        var client = new Notifier.NotifierClient(connection.Channel);
                        var request = new NotifyRequest()
                        {
                            Message = content.Message
                        };

                        try
                        {
                            var reply = client.Notify(request);
                            Console.WriteLine(
                                $"Notified subscriber {connection.Address} with {content.Message}. Response: {reply.IsSuccess}");
                        }
                        catch (RpcException rpcException)
                        {
                            if (rpcException.StatusCode == StatusCode.Internal)
                            {
                                _connectionStorageService.Remove(connection.Address);
                            }

                            Console.WriteLine($"RPC Error notifying subscriber {connection.Address}. {rpcException.Message}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error notifying subscriber {connection.Address}. {e.Message}");
                        }
                    }
                }
            }
        }
    }
}
