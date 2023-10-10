using System.Collections.Concurrent;
using Broker.Models;
using Broker.Services.Interfaces;

namespace Broker.Services
{
    public class ContentStorageService : IContentStorageService
    {
        private readonly ConcurrentQueue<Content> _contents;
        private readonly List<string> _savedTopics;

        public ContentStorageService()
        {
            _contents = new ConcurrentQueue<Content>();
            _savedTopics = new List<string>();
        }

        public void Add(Content content)
        {
            _contents.Enqueue(content);
        }

        public Content GetNext()
        {
            _contents.TryDequeue(out var content);

            return content;
        }

        public bool IsEmpty()
        {
            return _contents.IsEmpty;
        }
        
        public void AddTopic(string topic)
        {
            if (!_savedTopics.Contains(topic))
            {
                _savedTopics.Add(topic);
            }
        }

        public List<string> GetSavedTopics()
        {
            return _savedTopics;
        }
    }
}
