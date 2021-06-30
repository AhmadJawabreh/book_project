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
        private Message message;

        public Reader() 
        {
            this._connectionFactory = this.CreateConnection();
            this._connection = this._connectionFactory.CreateConnection();
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
            IModel channel = this._connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, snapShot) =>
            {
                var messageContent = snapShot.Body.ToArray();
                string content = Encoding.UTF8.GetString(messageContent);
                message = JsonConvert.DeserializeObject<Message>(content);
            };
            channel.BasicConsume(queue: Configuration.BookQueue,
                                 autoAck: true,
                                 consumer: consumer);
            return message;
        }
    }
}
