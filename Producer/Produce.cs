using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Producer
{

    public interface ISender
    {
        public ConnectionFactory CreateConnection();

        public void Send(object entity);
    }

    public class Sender : ISender
    {
        private IConnection _connection;

        private readonly ConnectionFactory _connectionFactory;

        public Sender()
        {
            this._connectionFactory = this.CreateConnection();
            this._connection = this._connectionFactory.CreateConnection();
        }

        public void Send(object entity)
        {
            IModel channel = this._connection.CreateModel();
            string content = JsonConvert.SerializeObject(entity);
            var messageContent = Encoding.UTF8.GetBytes(content);
            channel.BasicPublish(exchange: string.Empty, routingKey: SenderConfiguration.BookQueue, basicProperties: null, body: messageContent);
        }

        public ConnectionFactory CreateConnection()
        {
            return new ConnectionFactory()
            {
                HostName = SenderConfiguration.HostName,
                UserName = SenderConfiguration.UserName,
                Password = SenderConfiguration.Password,
                Port = SenderConfiguration.Port,
            };
        }
    }
}
