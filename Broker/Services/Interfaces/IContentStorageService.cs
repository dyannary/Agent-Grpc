using Broker.Models;

namespace Broker.Services.Interfaces
{
    public interface IContentStorageService
    {
        void Add(Content content);
        List<string> GetSavedTopics(); 
        void AddTopic(string topic);
        Content GetNext();

        bool IsEmpty();
    }
}
