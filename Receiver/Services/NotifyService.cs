using Grpc.Core;
using GrpcsAgent;

namespace Receiver.Services
{
    public class NotifyService : Notifier.NotifierBase
    {
        public override Task<NotifyReply> Notify(NotifyRequest request, ServerCallContext context)
        {
            Console.WriteLine("Received: " + request.Message);

            return Task.FromResult(new NotifyReply()
            {
                IsSuccess = true
            });
        }
    }
}
