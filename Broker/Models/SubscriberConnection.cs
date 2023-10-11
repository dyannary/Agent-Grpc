using Grpc.Net.Client;
using GrpcsAgent;
using Resources;

namespace Broker.Models
{
    public class SubscriberConnection
    {
        public SubscriberConnection(string address, string topic)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            Address = address;
            Topic = topic;
            Channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions { HttpHandler = httpHandler });
        }
        public string Address { get; }
        public string Topic { get; }

        public GrpcChannel Channel { get; }
    }
}
