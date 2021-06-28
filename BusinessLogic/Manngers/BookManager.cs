namespace BusinessLogic
{
    using BusinessLogic.Mappers;
    using Contract.Exceptions;
    using Entities;
    using Filters;
    using Models;
    using Repoistories;
    using Resources;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBookManager
    {
        List<BookResource> GetAll(Filter filter);

        Task<BookResource> GetByIdAsync(long Id);

        Task<BookResource> InsertAsync(BookModel bookModel);

        Task<BookResource> UpdateAsync(BookModel bookModel);

        Task<BookResource> GetBookWithAuthorsAndPublisher(Filter bookFilter, long Id);

        Task Delete(long Id);
    }

    public class BookManager : IBookManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookManager(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<BookResource> GetAll(Filter filter)
        {

            if (filter.PageNumber <= 0)
            {
                throw new InvalidArgumentException("Page Number must be more than 0.");
            }

            if (filter.PageSize <= 10)
            {
                throw new InvalidArgumentException("Page Size must be more than 10.");
            }

            List<Book> books = _unitOfWork.Books.GetAll(filter);

            return BookMapper.ToResources(books);
        }

        public async Task<BookResource> GetByIdAsync(long Id)
        {
            Book book = await _unitOfWork.Books.GetById(Id);
            if (book == null)
            {
                throw new NotFoundException("This Book does not found");
            }
            return BookMapper.ToResource(book);
        }

        public async Task<BookResource> InsertAsync(BookModel bookModel)
        {

            Book _book = _unitOfWork.Books.FirstOrDefalut(item => item.Name == bookModel?.Name);
            if (_book != null)
                throw new DubplicateDataException("Book Name already exist");


            // To DO:
            Filter filter = new Filter();
            filter.PageNumber = 1;
            filter.PageSize = 40;
         
            Book book = new Book();
            book = BookMapper.ToEntity(book, bookModel);
            await _unitOfWork.Books.Create(book);
            await this._unitOfWork.Save();
            return BookMapper.ToResource(book);
        }

        public async Task<BookResource> UpdateAsync(BookModel bookModel)
        {
            Book book = await _unitOfWork.Books.GetById(bookModel.Id);
            if (book == null) throw new NotFoundException("This Book does not found");




            Filter filter = new Filter() { PageNumber = 1, PageSize = 40 };
          

            book = BookMapper.ToEntity(book, bookModel);
            _unitOfWork.Books.Update(book);
            await this._unitOfWork.Save();
            return BookMapper.ToResource(book);
        }

        public async Task Delete(long Id)
        {
            Book book = await _unitOfWork.Books.GetById(Id);
            if (book == null)
            {
                throw new NotFoundException("This Book does not found");
            }
            _unitOfWork.Books.Delete(book);
            await this._unitOfWork.Save();
        }

        public async Task<BookResource> GetBookWithAuthorsAndPublisher(Filter bookFilter, long Id)
        {
            Book book = await _unitOfWork.Books.GetBookWithAuthorsAndPublisher(bookFilter, Id);
            if (book == null)
            {
                throw new NotFoundException("This Book does not found");
            }
            return BookMapper.ToResource(book); ;
        }
    }
}
