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
        List<PublisherResource> GetAll(Filter filter);
        Task<PublisherResource> GetByIdAsync(long Id);
        Task<PublisherResource> InsertAsync(PublisherModel publisherModel);
        Task<PublisherResource> UpdateAsync(PublisherModel publisherModel);
        Task DeleteAsync(long Id);
    }

    public class PublisherManager : IPublisherManager
    {
        private readonly IUnitOfWork _UnitOfWork;

        public PublisherManager(IUnitOfWork UnitOfWork)
        {
            this._UnitOfWork = UnitOfWork;
        }

        public  List<PublisherResource> GetAll(Filter filter)
        {
            if (filter.PageNumber <= 0)
            {
                throw new InvalidArgumentException("Page Number must be more than 0.");
            }

            if (filter.PageSize <= 10)
            {
                throw new InvalidArgumentException("Page Size must be more than 10.");
            }

            List<Publisher> publishers =  _UnitOfWork.Publishers.GetAll(filter);
            return PublisherMapper.ToResources(publishers);
        }

        public async Task<PublisherResource> GetByIdAsync(long Id)
        {

            Publisher publisher = await _UnitOfWork.Publishers.GetById(Id);
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
            await _UnitOfWork.Publishers.Create(publisher);
            await _UnitOfWork.Save();
            return PublisherMapper.ToResource(publisher);
        }

        public async Task<PublisherResource> UpdateAsync(PublisherModel publisherModel)
        {
            Publisher publisher = await _UnitOfWork.Publishers.GetById(publisherModel.Id);
            if (publisher == null)
            {
                throw new NotFoundException("This Publisher does not found");
            }
            publisher = PublisherMapper.ToEntity(publisher, publisherModel);
            _UnitOfWork.Publishers.Update(publisher);
            await _UnitOfWork.Save();
            return PublisherMapper.ToResource(publisher);
        }

        public async Task DeleteAsync(long Id)
        {
            Publisher publisher = await _UnitOfWork.Publishers.GetById(Id);
            if (publisher == null)
            {
                throw new NotFoundException("This Publisher does not found");
            }
            _UnitOfWork.Publishers.Delete(publisher);
            await _UnitOfWork.Save();
        }
    }
}
