namespace Consumer.Handler
{
    using BusinessLogic;
    using BusinessLogic.Mappers;
    using Consumer.Services;
    using Contract.RabbitMQ;
    using ENUM;
    using Models;
    using Resources;
    using System;
    using System.Threading.Tasks;

    public interface IMessageHandler
    {
        Task Handle(Message message);

        Task UpdatePublisher(Message message);

        Task UpdateAuthor(Message message);
    }

    public class MessageHandler : IMessageHandler
    {
        private readonly AuthorService _authorService;

        private readonly AuthorManager _authorManager;

        private readonly PublisherService _publisherService;

        private readonly PublisherManager _publisherManager;

        public MessageHandler(AuthorService authorService, AuthorManager authorManager, PublisherService publisherService, PublisherManager publisherManager)
        {

            this._authorService = authorService;
            this._authorManager = authorManager;
            this._publisherService = publisherService;
            this._publisherManager = publisherManager;
        }

        public async Task Handle(Message message)
        {
            if (message == null) return;
            switch (message?.dirtyEntityType)
            {
                case DirtyEntityType.Publisher:
                    await UpdatePublisher(message);
                    break;

                case DirtyEntityType.Author:
                    await UpdateAuthor(message);
                    break;
            }
        }

        public async Task UpdateAuthor(Message message)
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

        public async Task UpdatePublisher(Message message)
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
