using Grpc.Net.Client;
using Resources;
using GrpcsAgent;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Receiver.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<NotifyService>();

app.Start();

Subscribe(app);

Console.WriteLine("Press enter to exit...");
Console.ReadLine();

static void Subscribe(IHost host)
{
    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

    var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddress,
        new GrpcChannelOptions { HttpHandler = httpHandler });
    var client = new Subscriber.SubscriberClient(channel);

    var serverAddresses = host.Services.GetService<IServer>().Features.Get<IServerAddressesFeature>();
    Console.WriteLine("Server address:" + serverAddresses);

    if (serverAddresses != null)
    {
        var address = serverAddresses.Addresses.FirstOrDefault();
        if (!string.IsNullOrEmpty(address))
        {
            Console.WriteLine("Subscriber listening at: " + address);

            Console.Write("Enter the topic: ");
            var topic = Console.ReadLine().ToLower();

            var request = new SubscribeRequest()
            {
                Topic = topic,
                Address = address
            };

            try
            {
                var reply = client.Subscribe(request);
                Console.WriteLine("Subscribed reply: " + reply.IsSuccess);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error subscribing: " + e.Message);
            }
        }
    }
}

Console.WriteLine("Press enter to exit...");
Console.ReadLine();
