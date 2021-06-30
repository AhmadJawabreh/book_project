using BusinessLogic;
using BusinessLogic.Mappers;
using Consumer.Services;
using Contract.RabbitMQ;
using ENUM;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Resources;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Handler
{

    public interface IMessageHandler
    {
        ConnectionFactory GetConnection();
        void CreateConnection();

        void Listen();

        Task Handle(Message message);

        Task PublisherHandler(Message message);

        Task AuthorHandler(Message message);
    }

    public class MessageHandler : IMessageHandler
    {
        private readonly AuthorService _authorService;

        private readonly AuthorManager _authorManager;

        private readonly PublisherService _publisherService;

        private readonly PublisherManager _publisherManager;

        private IConnectionFactory _connectionFactory;
                private IConnection _connection;

        private Message message;

        public MessageHandler(AuthorService authorService, AuthorManager authorManager, PublisherService publisherService, PublisherManager publisherManager)
        {

            this._authorService = authorService;
            this._authorManager = authorManager;
            this._publisherService = publisherService;
            this._publisherManager = publisherManager;
        }

        public void CreateConnection()
        {
            _connectionFactory = GetConnection();
            _connection = this._connectionFactory.CreateConnection();
        }

        public ConnectionFactory GetConnection()
        {
            return new ConnectionFactory()
            {
                HostName = Configuration.HostName,
                UserName = Configuration.UserName,
                Password = Configuration.Password,
                Port = Configuration.Port,
            };
        }

        public void Listen()
        {
            IModel channel = this._connection.CreateModel();
            EventingBasicConsumer _consumer = new EventingBasicConsumer(channel);
            _consumer.Received += async (model, snapShot) =>
            {
                var messageContent = snapShot.Body.ToArray();
                string content = Encoding.UTF8.GetString(messageContent);
                message = JsonConvert.DeserializeObject<Message>(content);
                await Handle(message);
            };
            channel.BasicConsume(queue: Configuration.BookQueue,
                                 autoAck: true,
                                 consumer: _consumer);
            Console.ReadLine();
        }

        public async Task Handle(Message message)
        {
            if (message == null) return;
            switch (message?.dirtyEntityType)
            {
                case DirtyEntityType.Publisher:
                    await PublisherHandler(message);
                    break;

                case DirtyEntityType.Author:
                    await AuthorHandler(message);
                    break;
            }
        }

        public async Task AuthorHandler(Message message)
        {
            switch (message?.operationType)
            {
                case OperationType.Create:
                    AuthorResource newAuthorResource = await _authorService.GetById(message.id);
                    AuthorModel newAuthorModel = AuthorMapper.ToModel(newAuthorResource);
                    await _authorManager.InsertAsync(newAuthorModel);
                    break;
                case OperationType.Update:
                    AuthorResource updatedAuthorResource = await _authorService.GetById(message.id);
                    AuthorModel updatedAuthorModel = AuthorMapper.ToModel(updatedAuthorResource);
                    await _authorManager.UpdateAsync(updatedAuthorModel);
                    break;
                case OperationType.Delete:
                    await _authorManager.DeleteAsync(message.id);
                    break;
            }
        }

        public async Task PublisherHandler(Message message)
        {
            switch (message?.operationType)
            {
                case OperationType.Create:
                    PublisherResource newPublisherResource = await _publisherService.GetById(message.id);
                    PublisherModel newPublisherModel = PublisherMapper.ToModel(newPublisherResource);
                    await _publisherManager.InsertAsync(newPublisherModel);
                    break;
                case OperationType.Update:
                    PublisherResource updatedPublisherResource = await _publisherService.GetById(message.id);
                    PublisherModel updatedPublisherModel = PublisherMapper.ToModel(updatedPublisherResource);
                    await _publisherManager.UpdateAsync(updatedPublisherModel);
                    break;
                case OperationType.Delete:
                    await _publisherManager.DeleteAsync(message.id);
                    break;
            }
        }
    }
}
