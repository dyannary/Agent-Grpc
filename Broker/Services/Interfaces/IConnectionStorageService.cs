using Broker.Models;

namespace Broker.Services.Interfaces
{
    public interface IConnectionStorageService
    {
        void Add(SubscriberConnection subscriberConnection);

        void Remove(string address);

        IList<SubscriberConnection> GetConnectionsByTopic(string topic);
    }
}
