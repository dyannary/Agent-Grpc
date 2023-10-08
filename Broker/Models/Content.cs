namespace Broker.Models
{
    public class Content
    {
        public Content(string topic, string message)
        {
            Topic = topic;
            Message = message;
        }

        public string Topic { get; } = string.Empty;
        public string Message { get; } = string.Empty;
    }
}
