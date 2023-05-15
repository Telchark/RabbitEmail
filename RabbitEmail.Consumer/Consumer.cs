using RabbitEmail.Shared;
using RabbitMQ.Client;

namespace RabbitEmail.Consumer
{
    public class Consumer : RabbitMqClientBase
    {
        public Consumer(ConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }
    }
}
