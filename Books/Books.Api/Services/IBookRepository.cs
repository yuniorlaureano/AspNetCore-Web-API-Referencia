using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Api.Entities;

namespace Books.Api.Services
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookAsync(Guid bookId);
        void AddBook(Book book);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds);
    }
}