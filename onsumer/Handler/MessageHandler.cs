using BusinessLogic;
using BusinessLogic.Manngers;
using BusinessLogic.Mappers;
using Consumer.General;
using Consumer.Services;
using Contract.RabbitMQ;
using ENUM;
using Filters;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Handler
{
    public interface IMessageHandler
    {
        public ConnectionFactory GetConnection();

        public void CreateConnection();

        public void Listen();

        public Task Handle(Message message);

        public Task PublisherHandler(Message message);

        public Task AuthorHandler(Message message);

        public Task RestViewCaches();
    }

    public class MessageHandler : IMessageHandler
    {
        private readonly IAuthorService _authorService;

        private readonly IAuthorManager _authorManager;

        private readonly IPublisherService _publisherService;

        private readonly IPublisherManager _publisherManager;

        private readonly IHarvesterManager _harvesterManager;


        private IConnectionFactory _connectionFactory;

        private IConnection _connection;

        public Message message;

        public MessageHandler(IAuthorService authorService,
                IAuthorManager authorManager,
                IPublisherService publisherService,
                IPublisherManager publisherManager,
                IHarvesterManager harvesterManager
               )
        {
            _authorService = authorService;

            _authorManager = authorManager;

            _publisherService = publisherService;

            _publisherManager = publisherManager;

            _harvesterManager = harvesterManager;

        }

        public void CreateConnection()
        {
            _connectionFactory = GetConnection();

            _connection = _connectionFactory.CreateConnection();
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
            IModel channel = _connection.CreateModel();

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, snapShot) =>
            {
                var messageContent = snapShot.Body.ToArray();

                string content = Encoding.UTF8.GetString(messageContent);

                message = JsonConvert.DeserializeObject<Message>(content);

                await Handle(message);
            };

            channel.BasicConsume(queue: Configuration.BookQueue,
                                 autoAck: true,
                                 consumer: consumer);
            Console.ReadLine();
        }

        public async Task Handle(Message message)
        {
            if (message == null)
                return;

            switch (message?.DirtyEntityType)
            {
                case DirtyEntityType.Publisher:
                    await PublisherHandler(message);
                    break;

                case DirtyEntityType.Author:
                    await AuthorHandler(message);
                    break;

                case DirtyEntityType.None:
                    await RestViewCaches();
                    break;
            }
        }

        public async Task AuthorHandler(Message message)
        {
            switch (message?.OperationType)
            {
                case OperationType.Create:
                    AuthorResource newAuthorResource = await _authorService.GetById(message.Id);
                    AuthorModel newAuthorModel = AuthorMapper.ToModel(newAuthorResource);
                    await _authorManager.InsertAsync(newAuthorModel);
                    break;

                case OperationType.Update:
                    AuthorResource updatedAuthorResource = await _authorService.GetById(message.Id);
                    AuthorModel updatedAuthorModel = AuthorMapper.ToModel(updatedAuthorResource);
                    await _authorManager.UpdateAsync(updatedAuthorModel);
                    break;

                case OperationType.Delete:
                    await _authorManager.DeleteAsync(message.Id);
                    break;
            }
        }

        public async Task PublisherHandler(Message message)
        {
            switch (message?.OperationType)
            {
                case OperationType.Create:
                    PublisherResource newPublisherResource = await _publisherService.GetById(message.Id);
                    PublisherModel newPublisherModel = PublisherMapper.ToModel(newPublisherResource);
                    await _publisherManager.InsertAsync(newPublisherModel);
                    break;

                case OperationType.Update:
                    PublisherResource updatedPublisherResource = await _publisherService.GetById(message.Id);
                    PublisherModel updatedPublisherModel = PublisherMapper.ToModel(updatedPublisherResource);
                    await _publisherManager.UpdateAsync(updatedPublisherModel);
                    break;

                case OperationType.Delete:
                    await _publisherManager.DeleteAsync(message.Id);
                    break;
            }
        }

        public async Task RestViewCaches()
        {
            Filter filter = new Filter() { PageNumber = 1, PageSize = 200 };

            List<AuthorResource> authorResources = await _authorService.GetAll(filter);

            List<PublisherResource> publisherResources = await _publisherService.GetAll(filter);

            List<AuthorModel> authorModels = AuthorMapper.ToModels(authorResources);

            List<PublisherModel> publisherModels = PublisherMapper.ToModels(publisherResources);

            await _harvesterManager.ClearAllAuthorsAsync();

            await _harvesterManager.ClearAllPublisherAsync();

            await _harvesterManager.InsertAuthors(authorModels);

            await _harvesterManager.InsertPublishers(publisherModels);

            await _harvesterManager.Save();
        }
    }
}
