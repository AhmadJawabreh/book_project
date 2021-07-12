using BusinessLogic.Mappers;
using Contract.RabbitMQ;
using Entities;
using ENUM;
using Models;
using Producer;
using Repoistories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Manngers
{

    public interface IHarvesterManager
    {
        public Task Save();

        public void ResetViewCaches();

        public Task ClearAllAuthorsAsync();

        public Task ClearAllPublisherAsync();

        public Task InsertPublishers(List<PublisherModel> publishers);

        public Task InsertAuthors(List<AuthorModel> authors);
    }

    public class HarvesterManager : IHarvesterManager
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IProduce _produce;

        public HarvesterManager(IUnitOfWork unitOfWork, IProduce produce)
        {
            _unitOfWork = unitOfWork;
            _produce = produce;
        }

        public async Task Save()
        {
            await _unitOfWork.Save();
        }

        public void ResetViewCaches()
        {
            Message settingMessage = new Message()
            {
                OperationType = OperationType.Rest,
                DirtyEntityType = DirtyEntityType.None
            };
            _produce.CreateConnection();
            _produce.Send(settingMessage);
        }

        public async Task ClearAllAuthorsAsync()
        {
            List<Author> Authors = await _unitOfWork.Authors.GetAllAsync();
            foreach (Author author in Authors)
            {
                _unitOfWork.Authors.Delete(author);
            }
        }

        public async Task ClearAllPublisherAsync()
        {
            List<Publisher> publishers = await _unitOfWork.Publishers.GetAllAsync();
            foreach (Publisher publisher in publishers)
            {
                _unitOfWork.Publishers.Delete(publisher);
            }
        }

        public async Task InsertAuthors(List<AuthorModel> authorModels)
        {
            foreach (AuthorModel authorModel in authorModels)
            {
                Author author = new Author();
                author = AuthorMapper.ToEntity(author, authorModel);
                await _unitOfWork.Authors.Create(author);
            }
        }

        public async Task InsertPublishers(List<PublisherModel> publisherModels)
        {
            foreach (PublisherModel publisherModel in publisherModels)
            {
                Publisher publisher = new Publisher();
                publisher = PublisherMapper.ToEntity(publisher, publisherModel);
                await _unitOfWork.Publishers.Create(publisher);
            }
        }
    }
}
