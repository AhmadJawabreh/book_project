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

            //Check if this book name exist before.
            Book _book = _unitOfWork.Books.FirstOrDefalut(item => item.Name == bookModel?.Name);

            if (_book != null)
                throw new DubplicateDataException("Book Name already exist");

            //Check if this publisher exist.
            Publisher publisher = _unitOfWork.Publishers.FirstOrDefalut(item => item.Id == bookModel?.PublisherId);
            if (publisher == null)
                throw new NotFoundException("Publisher does not exist ");

            //Ensure that all these authors are exist on database.
            List<Author> Authors = _unitOfWork.Authors.Where(item => bookModel.AuthoIds.Contains(item.Id)).ToList();

            //Convert bookModel to Book.
            Book book = new Book();
            BookMapper.ToEntity(book, bookModel);

            //Insert the new book
            await _unitOfWork.Books.Create(book);

            //Save all changes in order to get book id.
            await _unitOfWork.Save();

            //Add all authorIds inside BookAuthor Table.
            foreach (Author author in Authors)
            {
                await _unitOfWork.BookAuthors.Create(new BookAuthor() { AuthorId = author.Id, BookId = book.Id });
            }

            //Save all changes.
            await _unitOfWork.Save();

            //convert book to book resource. 
            BookResource bookResource = BookMapper.ToResource(book);
            bookResource.Authors = AuthorMapper.ToResources(Authors);
            bookResource.publisher = PublisherMapper.ToResource(publisher);

            //Return book resource.
            return bookResource;
        }

        public async Task<BookResource> UpdateAsync(BookModel bookModel)
        {

            //Check if this book is exist.
            Book book = _unitOfWork.Books.FirstOrDefalut(item => item.Id == bookModel?.Id);
            if (book == null)
                throw new DubplicateDataException("This Book does not exist");

            //Check if this book name exist before.
            Book _book = _unitOfWork.Books.FirstOrDefalut(item => item.Name == bookModel?.Name);
            if (_book != null && _book.Id != bookModel.Id)
                throw new DubplicateDataException("Book Name already exist");

            //Check if this publisher exist.
            Publisher publisher = _unitOfWork.Publishers.FirstOrDefalut(item => item.Id == bookModel?.PublisherId);
            if (publisher == null)
                throw new NotFoundException("Publisher does not exist ");


            //Check that all BookAuthors are exist.
            List<BookAuthor> BookAuthors = _unitOfWork.BookAuthors.Where(item => bookModel.AuthoIds.Contains(item.AuthorId)).ToList();

            //Convert bookModel to Book.
            BookMapper.ToEntity(book, bookModel);
            book.BookAuthors = BookAuthors;

            //Update the old book
            _unitOfWork.Books.Update(book);

            //Save all changes.
            await _unitOfWork.Save();


            //Get authors to display them in back request.
            List<Author> Authors = _unitOfWork.Authors.Where(item => bookModel.AuthoIds.Contains(item.Id)).ToList();

            //convert book to book resource. 
            BookResource bookResource = BookMapper.ToResource(book);
            bookResource.Authors = AuthorMapper.ToResources(Authors);
            bookResource.publisher = PublisherMapper.ToResource(publisher);

            //Return book resource.
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
