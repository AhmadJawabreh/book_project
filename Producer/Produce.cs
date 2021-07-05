using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Producer
{

    public interface IProduce
    {
        public ConnectionFactory CreateConnection();

        public void Send(object entity);
    }

    public class Produce : IProduce
    {
        private IConnection _connection;

        private readonly ConnectionFactory _connectionFactory;

        public Produce()
        {
            this._connectionFactory = this.CreateConnection();
            this._connection = this._connectionFactory.CreateConnection();
        }

        public void Send(object entity)
        {
            IModel channel = this._connection.CreateModel();
            string content = JsonConvert.SerializeObject(entity);
            var messageContent = Encoding.UTF8.GetBytes(content);
            channel.BasicPublish(exchange: string.Empty, routingKey: ProducerConfiguration.BookQueue, basicProperties: null, body: messageContent);
        }

        public ConnectionFactory CreateConnection()
        {
            return new ConnectionFactory()
            {
                HostName = ProducerConfiguration.HostName,
                UserName = ProducerConfiguration.UserName,
                Password = ProducerConfiguration.Password,
                Port = ProducerConfiguration.Port,
            };
        }
    }
}
