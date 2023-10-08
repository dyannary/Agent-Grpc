﻿using Grpc.Net.Client;

namespace Broker.Models
{
    public class SubscriberConnection
    {
        public SubscriberConnection(string address, string topic)
        {
            Address = address;
            Topic = topic;
            Channel = GrpcChannel.ForAddress(address);
        }
        public string Address { get; }
        public string Topic { get; }

        public GrpcChannel Channel { get; }
    }
}
