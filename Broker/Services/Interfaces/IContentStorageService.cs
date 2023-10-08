using Broker.Models;

namespace Broker.Services.Interfaces
{
    public interface IContentStorageService
    {
        void Add(Content content);

        Content GetNext();

        bool IsEmpty();
    }
}
