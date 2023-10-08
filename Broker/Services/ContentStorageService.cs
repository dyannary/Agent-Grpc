using System.Collections.Concurrent;
using Broker.Models;
using Broker.Services.Interfaces;

namespace Broker.Services
{
    public class ContentStorageService : IContentStorageService
    {
        private readonly ConcurrentQueue<Content> _contents;

        public ContentStorageService()
        {
            _contents = new ConcurrentQueue<Content>();
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
    }
}
