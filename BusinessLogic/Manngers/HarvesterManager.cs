using Contract.RabbitMQ;
using Entities;
using ENUM;
using Producer;
using Repoistories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Manngers
{


    public interface IHarvester
    {
        void ClearViewCaches();

        void ClearAllAuthors();

        void ClearAllPublisher();

        Task InsertPublishers(List<Publisher> Publishers);

        Task InsertAuthors(List<Author> Authors);
    }

    public class HarvesterManager : IHarvester
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IProduce _produce;

        public HarvesterManager(IUnitOfWork unitOfWork, IProduce produce)
        {
            this._unitOfWork = unitOfWork;
            this._produce = produce;
        }

        public void ClearViewCaches()
        {
            Message SettingMessage = new Message()
            {
                operationType = OperationType.Rest
            };
            this._produce.CreateConnection();
            this._produce.Send(SettingMessage);
        }

        public void ClearAllAuthors()
        {
            List<Author> Authors = this._unitOfWork.Authors.GetAll();
            foreach (Author author in Authors)
            {
                this._unitOfWork.Authors.Delete(author);
            }
        }

        public void ClearAllPublisher()
        {
            List<Publisher> Publishers = this._unitOfWork.Publishers.GetAll();
            foreach (Publisher publisher in Publishers)
            {
                this._unitOfWork.Publishers.Delete(publisher);
            }
        }

        public async Task InsertAuthors(List<Author> Authors)
        {
            foreach (Author author in Authors)
            {
                await _unitOfWork.Authors.Create(author);
            }
        }

        public async Task InsertPublishers(List<Publisher> Publishers)
        {
            foreach (Publisher publisher in Publishers)
            {
                await _unitOfWork.Publishers.Create(publisher);
            }
        }
    }
}
