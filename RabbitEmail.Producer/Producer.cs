using RabbitEmail.Shared;
using RabbitMQ.Client;
using System.Text;

namespace RabbitEmail.Producer
{
    public class Producer : RabbitMqClientBase
    {
        public Producer(ConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        public void Publish(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            var properties = Channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 1;
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Channel.BasicPublish(exchange: ExchangeName, routingKey: routingKey, body: body, basicProperties: properties);
        }
    }
}