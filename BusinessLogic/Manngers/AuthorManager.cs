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
    public interface IAuthorManager
    {
        public List<AuthorResource> GetAll(Filter filter);

        public Task<AuthorResource> GetByIdAsync(long id);

        public Task<AuthorResource> InsertAsync(AuthorModel authorModel);

        public Task<AuthorResource> UpdateAsync(AuthorModel authorModel);

        public Task DeleteAsync(long id);
    }

    public class AuthorManager : IAuthorManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<AuthorResource> GetAll(Filter filter)
        {

            if (filter?.PageNumber <= 0)
            {
                throw new InvalidArgumentException("Page Number must be more than 0.");
            }

            if (filter?.PageSize < 10)
            {
                throw new InvalidArgumentException("Page Size must be more than 10.");
            }

            List<Author> authors = _unitOfWork.Authors.GetAll(filter);
            return AuthorMapper.ToResources(authors);
        }

        public async Task<AuthorResource> GetByIdAsync(long id)
        {
            Author author = await _unitOfWork.Authors.GetById(id);
            if (author == null)
            {
                throw new NotFoundException("This Author does not found.");
            }
            return AuthorMapper.ToResource(author);
        }

        public async Task<AuthorResource> InsertAsync(AuthorModel authorModel)
        {
            Author author = new Author();
            author = AuthorMapper.ToEntity(author, authorModel);
            await _unitOfWork.Authors.Create(author);
            await _unitOfWork.Save();
            return AuthorMapper.ToResource(author);
        }

        public async Task<AuthorResource> UpdateAsync(AuthorModel authorModel)
        {
            Author author = await _unitOfWork.Authors.GetById(authorModel.Id);
            if (author == null)
            {
                throw new NotFoundException("This Author does not found.");
            }
            author = AuthorMapper.ToEntity(author, authorModel);
            _unitOfWork.Authors.Update(author);
            await _unitOfWork.Save();
            return AuthorMapper.ToResource(author);
        }

        public async Task DeleteAsync(long id)
        {
            Author author = await _unitOfWork.Authors.GetById(id);
            if (author == null)
            {
                throw new NotFoundException("This Author does not found.");
            }
            _unitOfWork.Authors.Delete(author);
            await _unitOfWork.Save();
        }
    }
}
