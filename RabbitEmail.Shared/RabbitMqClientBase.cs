using RabbitMQ.Client;

namespace RabbitEmail.Shared
{
    public abstract class RabbitMqClientBase : IDisposable
    {
        public string QueueName { get; protected set; } = "EmailQ";
        public string ExchangeName { get; protected set; } = "EmailEx";

        public IModel? Channel { get; private set; }
        private IConnection? _connection;
        private readonly ConnectionFactory _connectionFactory;


        protected RabbitMqClientBase(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void ConnectToServer()
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel == null || Channel.IsOpen == false)
            {
                Channel = _connection.CreateModel();
                Channel.ExchangeDeclare(ExchangeName, "direct", true, false);
                Channel.QueueDeclare(QueueName, true, false, false);
                foreach (Bindings protocolType in Enum.GetValues(typeof(Bindings)))
                {
                    Channel.QueueBind(QueueName, ExchangeName, protocolType.ToString());
                }
            }
        }

        public void Dispose()
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null;

            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
    }
}
