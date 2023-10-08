using Broker.Models;
using Broker.Services.Interfaces;

namespace Broker.Services
{
    public class ConnectionStorageService : IConnectionStorageService
    {
        private readonly List<SubscriberConnection> _subscriberConnections;
        private readonly object _locker;

        public ConnectionStorageService()
        {
            _subscriberConnections = new List<SubscriberConnection>();
            _locker = new object();
        }

        public void Add(SubscriberConnection subscriberConnection)
        {
            lock (_locker)
            {
                _subscriberConnections.Add(subscriberConnection);   
            }
        }

        public IList<SubscriberConnection> GetConnectionsByTopic(string topic)
        {
            lock (_locker)
            {
                var filteredConnections = _subscriberConnections.Where(x => x.Topic == topic).ToList();
                return filteredConnections;
            }
        }

        public void Remove(string address)
        {
            lock (_locker)
            {
                _subscriberConnections.RemoveAll(x => x.Address == address);
            }
        }
    }
}
