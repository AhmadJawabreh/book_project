using BusinessLogic.Mappers;
using Contract.Exceptions;
using Entities;
using Filters;
using Models;
using Repoistories;
using Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IPublisherManager
    {
        public List<PublisherResource> GetAll(Filter filter);

        public Task<PublisherResource> GetByIdAsync(long id);

        public Task<PublisherResource> InsertAsync(PublisherModel publisherModel);

        public Task<PublisherResource> UpdateAsync(PublisherModel publisherModel);

        public Task DeleteAsync(long id);
    }

    public class PublisherManager : IPublisherManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublisherManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<PublisherResource> GetAll(Filter filter)
        {
            if (filter.PageNumber <= 0)
            {
                throw new InvalidArgumentException("Page Number must be more than 0.");
            }

            if (filter.PageSize <= 10)
            {
                throw new InvalidArgumentException("Page Size must be more than 10.");
            }

            List<Publisher> publishers = _unitOfWork.Publishers.GetAll(filter);

            return PublisherMapper.ToResources(publishers);
        }

        public async Task<PublisherResource> GetByIdAsync(long id)
        {

            Publisher publisher = await _unitOfWork.Publishers.GetById(id);
            if (publisher == null)
            {
                throw new NotFoundException("This publisher does not found");
            }

            return PublisherMapper.ToResource(publisher);
        }

        public async Task<PublisherResource> InsertAsync(PublisherModel publisherModel)
        {

            bool isEmailOrPhoneEmpty = string.IsNullOrEmpty(publisherModel.Email) || string.IsNullOrEmpty(publisherModel.Phone);
            if (isEmailOrPhoneEmpty)
            {
                throw new InvalidArgumentException("You Should enter Phone or Email");
            }

            Publisher publisher = new Publisher();
            publisher = PublisherMapper.ToEntity(publisher, publisherModel);

            await _unitOfWork.Publishers.Create(publisher);

            await _unitOfWork.Save();

            return PublisherMapper.ToResource(publisher);
        }

        public async Task<PublisherResource> UpdateAsync(PublisherModel publisherModel)
        {
            Publisher publisher = await _unitOfWork.Publishers.GetById(publisherModel.Id);
            if (publisher == null)
            {
                throw new NotFoundException("This Publisher does not found");
            }

            publisher = PublisherMapper.ToEntity(publisher, publisherModel);

            _unitOfWork.Publishers.Update(publisher);

            await _unitOfWork.Save();

            return PublisherMapper.ToResource(publisher);
        }

        public async Task DeleteAsync(long id)
        {
            Publisher publisher = await _unitOfWork.Publishers.GetById(id);
            if (publisher == null)
            {
                throw new NotFoundException("This Publisher does not found");
            }

            _unitOfWork.Publishers.Delete(publisher);

            await _unitOfWork.Save();
        }
    }
}
