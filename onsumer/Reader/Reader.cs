using Contract.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{

    public interface IReader
    {
        public ConnectionFactory CreateConnection();

        public Message GetMessage();
    }

    public class Reader : IReader
    {
        private IConnection _connection;

        private readonly ConnectionFactory _connectionFactory;

        private readonly IModel _channel;

        private Message message;

        public Reader()
        {
            this._connectionFactory = this.CreateConnection();
            this._connection = this._connectionFactory.CreateConnection();
            this._channel = this._connection.CreateModel();
        }

        public ConnectionFactory CreateConnection()
        {
            return new ConnectionFactory()
            {
                HostName = Configuration.HostName,
                UserName = Configuration.UserName,
                Password = Configuration.Password,
                Port = Configuration.Port,
            };
        }

        public Message GetMessage()
        {
            var consumer = new EventingBasicConsumer(this._channel);
            consumer.Received += (model, snapShot) =>
            {
                var messageContent = snapShot.Body.ToArray();
                string content = Encoding.UTF8.GetString(messageContent);
                message = JsonConvert.DeserializeObject<Message>(content);
            };
            this._channel.BasicConsume(queue: Configuration.BookQueue,
                                 autoAck: true,
                                 consumer: consumer);
            return message;
        }
    }
}
