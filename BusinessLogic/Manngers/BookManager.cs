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


namespace BusinessLogic
{
    public interface IBookManager
    {
        public Task<List<BookResource>> GetAllAsync(Filter filter);

        public Task<BookResource> GetByIdAsync(long id);

        public Task<BookResource> InsertAsync(BookModel bookModel);

        public Task<BookResource> UpdateAsync(BookModel bookModel);

        public Task Delete(long id);
    }

    public class BookManager : IBookManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            List<Book> books = _unitOfWork.Books.GetAll(filter);

            List<BookResource> bookResources = new List<BookResource>();

            foreach (Book book in books)
            {
                Publisher publisher = await _unitOfWork.Publishers.GetById((long)book.PublisherId);

                List<long> authorIds = book.BookAuthors.Select(item => item.AuthorId).ToList();

                List<Author> authors = _unitOfWork.Authors.Where(item => authorIds.Contains(item.Id)).ToList();

                BookResource bookResource = BookMapper.ToResource(book);
                bookResource.Publisher = PublisherMapper.ToResource(publisher);
                bookResource.Authors = AuthorMapper.ToResources(authors);

                bookResources.Add(bookResource);
            }

            return bookResources;
        }

        public async Task<BookResource> GetByIdAsync(long id)
        {

            Book book = await _unitOfWork.Books.GetBookWithAuthors(id);

            if (book == null)
            {
                throw new NotFoundException("This Book does not found");
            }

            Publisher publisher = await _unitOfWork.Publishers.GetById((long)book.PublisherId);

            List<long> authorIds = book.BookAuthors.Select(item => item.AuthorId).ToList();

            List<Author> authors = _unitOfWork.Authors.Where(item => authorIds.Contains(item.Id)).ToList();

            BookResource bookResource = BookMapper.ToResource(book);
            bookResource.Publisher = PublisherMapper.ToResource(publisher);
            bookResource.Authors = AuthorMapper.ToResources(authors);

            return bookResource;
        }

        public async Task<BookResource> InsertAsync(BookModel bookModel)
        {

            string bookName = _unitOfWork.Books.FirstOrDefalut(item => item.Name == bookModel?.Name)?.Name;
            if (!string.IsNullOrEmpty(bookName))
                throw new DubplicateDataException("Book Name already exist");


            Publisher publisher = _unitOfWork.Publishers.FirstOrDefalut(item => item.Id == bookModel?.PublisherId);
            if (publisher == null)
                throw new NotFoundException("Publisher does not exist ");


            List<Author> authors = _unitOfWork.Authors.Where(item => bookModel.AuthorIds.Contains(item.Id)).ToList();

            Book book = BookMapper.ToEntity(new Book(), bookModel);

            await _unitOfWork.Books.Create(book);

            await _unitOfWork.Save();

            foreach (Author author in authors)
            {
                await _unitOfWork.BookAuthors.Create(new BookAuthor() { AuthorId = author.Id, BookId = book.Id });
            }

            await _unitOfWork.Save();

            BookResource bookResource = BookMapper.ToResource(book);
            bookResource.Authors = AuthorMapper.ToResources(authors);
            bookResource.Publisher = PublisherMapper.ToResource(publisher);

            return bookResource;
        }

        public async Task<BookResource> UpdateAsync(BookModel bookModel)
        {

            Book book = await _unitOfWork.Books.GetBookWithAuthors((long)bookModel?.Id);
            if (book == null)
                throw new DubplicateDataException("This Book does not exist");


            Book oldBook = _unitOfWork.Books.FirstOrDefalut(item => item.Name == bookModel?.Name);
            if (oldBook != null && oldBook.Id != bookModel.Id)
                throw new DubplicateDataException("Book Name already exist");


            Publisher publisher = _unitOfWork.Publishers.FirstOrDefalut(item => item.Id == bookModel?.PublisherId);
            if (publisher == null)
                throw new NotFoundException("Publisher does not exist ");


            List<Author> authors = _unitOfWork.Authors.Where(item => bookModel.AuthorIds.Contains(item.Id)).ToList();

            List<BookAuthor> bookAuthors = authors.Select(item => new BookAuthor { AuthorId = item.Id, BookId = book.Id }).ToList();

            BookMapper.ToEntity(book, bookModel);

            book.BookAuthors = bookAuthors;

            _unitOfWork.Books.Update(book);

            await _unitOfWork.Save();

            BookResource bookResource = BookMapper.ToResource(book);
            bookResource.Authors = AuthorMapper.ToResources(authors);
            bookResource.Publisher = PublisherMapper.ToResource(publisher);

            return bookResource;
        }

        public async Task Delete(long id)
        {
            Book book = await _unitOfWork.Books.GetById(id);

            if (book == null)
            {
                throw new NotFoundException("This Book does not found");
            }

            _unitOfWork.Books.Delete(book);

            await _unitOfWork.Save();
        }
    }
}
