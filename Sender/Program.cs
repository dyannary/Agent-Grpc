using Grpc.Core;
using Grpc.Net.Client;
using GrpcsAgent;
using Resources;

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

Console.WriteLine("Publisher");

//var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress);
var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress, new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new Publisher.PublisherClient(channel);

while (true)
{
    Console.Write("Enter the topic: ");
    var topic = Console.ReadLine().ToLower();

    Console.Write("Enter the message: ");
    var message = Console.ReadLine();

    var request = new PublishRequest()
    {
        Topic = topic,
        Message = message
    };

    try
    {
        var reply = await client.PublishMessageAsync(request);
        Console.WriteLine("Publish Reply: " + reply.IsSuccess);
    }
    catch (Exception e)
    {
        Console.WriteLine("Error publishing the message: " + e.Message);
    }

}