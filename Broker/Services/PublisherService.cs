using Grpc.Core;
using GrpcsAgent;
using Broker.Models;
using Broker.Services.Interfaces;

namespace Broker.Services
{
    public class PublisherService : Publisher.PublisherBase
    {
        private readonly IContentStorageService _messageStorageService;

        public PublisherService(IContentStorageService messageStorageService)
        {
            _messageStorageService = messageStorageService;
        }

        public override Task<PublishReply> PublishMessage(PublishRequest request, ServerCallContext context)
        {
            Console.WriteLine("Received: " + request.Topic + " " + request.Message);

            var content = new Content(request.Topic, request.Message);
            _messageStorageService.Add(content);
            _messageStorageService.AddTopic(content.Topic);


            return Task.FromResult(new PublishReply()
            {
                IsSuccess = true
            });
        }
        
        public override Task<TopicList> GetSavedTopics(GetSavedTopicsRequest request, ServerCallContext context)
        {
            List<string> savedTopics = _messageStorageService.GetSavedTopics();

            var topicList = new TopicList();
            topicList.Topics.AddRange(savedTopics);

            return Task.FromResult(topicList);
        }
    }
}
