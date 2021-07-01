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
        Task<List<BookResource>> GetAllAsync(Filter filter);

        Task<BookResource> GetByIdAsync(long Id);

        Task<BookResource> InsertAsync(BookModel bookModel);

        Task<BookResource> UpdateAsync(BookModel bookModel);

        Task Delete(long Id);
    }

    public class BookManager : IBookManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookManager(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<BookResource>> GetAllAsync(Filter filter)
        {

            if (filter.PageNumber <= 0)
            {
                throw new InvalidArgumentException("Page Number must be more than 0.");
            }

            if (filter.PageSize <= 10)
            {
                throw new InvalidArgumentException("Page Size must be more than 10.");
            }

            List<Book> Books = _unitOfWork.Books.GetAll(filter);

            List<BookResource> BookResources = new List<BookResource>();

            foreach (Book book in Books)
            {
                Publisher publisher = await _unitOfWork.Publishers.GetById((long)book.PublisherId);

                List<long> authorIds = book.BookAuthors.Select(item => item.AuthorId).ToList();

                List<Author> authors = _unitOfWork.Authors.Where(item => authorIds.Contains(item.Id)).ToList();

                BookResource bookResource = BookMapper.ToResource(book);

                bookResource.publisher = PublisherMapper.ToResource(publisher);

                bookResource.Authors = AuthorMapper.ToResources(authors);

                BookResources.Add(bookResource);
            }


            return BookResources;
        }

        public async Task<BookResource> GetByIdAsync(long Id)
        {

            Book book = await _unitOfWork.Books.GetBookWithAuthors(Id);

            if (book == null)
            {
                throw new NotFoundException("This Book does not found");
            }

            Publisher publisher = await _unitOfWork.Publishers.GetById((long)book.PublisherId);

            List<long> authorIds = book.BookAuthors.Select(item => item.AuthorId).ToList();

            List<Author> authors = _unitOfWork.Authors.Where(item => authorIds.Contains(item.Id)).ToList();

            BookResource bookResource = BookMapper.ToResource(book);

            bookResource.publisher = PublisherMapper.ToResource(publisher);

            bookResource.Authors = AuthorMapper.ToResources(authors);

            return bookResource;
        }

        public async Task<BookResource> InsertAsync(BookModel bookModel)
        {

            Book _book = _unitOfWork.Books.FirstOrDefalut(item => item.Name == bookModel?.Name);

            if (_book != null)
                throw new DubplicateDataException("Book Name already exist");

            Publisher publisher = _unitOfWork.Publishers.FirstOrDefalut(item => item.Id == bookModel?.PublisherId);
            if (publisher == null)
                throw new NotFoundException("Publisher does not exist ");

            List<Author> Authors = _unitOfWork.Authors.Where(item => bookModel.AuthorIds.Contains(item.Id)).ToList();

            Book book = BookMapper.ToEntity(new Book(), bookModel);

            await _unitOfWork.Books.Create(book);

            await _unitOfWork.Save();

            foreach (Author author in Authors)
            {
                await _unitOfWork.BookAuthors.Create(new BookAuthor() { AuthorId = author.Id, BookId = book.Id });
            }

            await _unitOfWork.Save();

            BookResource bookResource = BookMapper.ToResource(book);
            bookResource.Authors = AuthorMapper.ToResources(Authors);
            bookResource.publisher = PublisherMapper.ToResource(publisher);
            return bookResource;
        }

        public async Task<BookResource> UpdateAsync(BookModel bookModel)
        {

            Book book = await _unitOfWork.Books.GetBookWithAuthors((long)bookModel?.Id);
            if (book == null)
                throw new DubplicateDataException("This Book does not exist");

            Book _book = _unitOfWork.Books.FirstOrDefalut(item => item.Name == bookModel?.Name);
            if (_book != null && _book.Id != bookModel.Id)
                throw new DubplicateDataException("Book Name already exist");

            Publisher publisher = _unitOfWork.Publishers.FirstOrDefalut(item => item.Id == bookModel?.PublisherId);
            if (publisher == null)
                throw new NotFoundException("Publisher does not exist ");

            List<Author> Authors = _unitOfWork.Authors.Where(item => bookModel.AuthorIds.Contains(item.Id)).ToList();

            List<BookAuthor> BookAuthors = Authors.Select(item => new BookAuthor { AuthorId = item.Id, BookId = book.Id }).ToList();

            BookMapper.ToEntity(book, bookModel);

            book.BookAuthors = BookAuthors;

            _unitOfWork.Books.Update(book);

            await _unitOfWork.Save();

            BookResource bookResource = BookMapper.ToResource(book);

            bookResource.Authors = AuthorMapper.ToResources(Authors);

            bookResource.publisher = PublisherMapper.ToResource(publisher);

            return bookResource;
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
    }
}
