using Entities;
using Models;
using Resources;
using System.Collections.Generic;

namespace BusinessLogic.Mappers
{


    public class BookMapper
    {
        public static List<BookResource> ToResources(IEnumerable<Book> books)
        {
            List<BookResource> BookResourcess = new List<BookResource>();
            foreach (var Item in books)
            {
                BookResourcess.Add(
                     new BookResource
                     {
                         Id = Item.Id,
                         Name = Item?.Name,
                         ReleaseDate = Item.ReleaseDate
                     }
                    );
            }
            return BookResourcess;
        }

        public static BookResource ToResource(Book book)
        {
            BookResource bookResource = new BookResource();
            bookResource.Id = book.Id;
            bookResource.Name = book.Name;
            bookResource.ReleaseDate = book.ReleaseDate;
            return bookResource;
        }

        public static Book ToEntity(Book book, BookModel bookModel)
        {
            book.Id = bookModel.Id;
            book.Name = bookModel.Name;
            book.ReleaseDate = bookModel.ReleaseDate;
            book.PublisherId = bookModel.PublisherId;
            return book;
        }
    }
}
