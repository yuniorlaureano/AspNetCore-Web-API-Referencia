using AutoMapper;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Api.Models;
using Books.Api.Entities;
using Books.Api.Helpers;
using Books.Api.Filters;

namespace Books.Api.Controllers
{
    [Route("api/bookcollections")]
    [ApiController]
    public class BookCollectionController: ControllerBase
    {
        private IBookRepository _bookRepository;
        private IMapper _mapper;

        public BookCollectionController(IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet("({bookIds})", Name = "GetBookCollection")]
        [BooksResultFilter]
        public async Task<IActionResult> GetBookCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> bookIds)
        {
            var bookEntities = await _bookRepository.GetBooksAsync(bookIds);

            if (bookEntities.Count() != bookIds.Count())
            {
                return NotFound();
            }

            return Ok(bookEntities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookCollection(IEnumerable<BookForCreationDto> bookForCreation)
        {
            var mappedBook = _mapper.Map<IEnumerable<Book>>(bookForCreation);

            foreach (var bookEntity in mappedBook)
            {
                _bookRepository.AddBook(bookEntity);
            }

            await _bookRepository.SaveChangesAsync();

            var bookToReturn = mappedBook.Select(b => b.Id);
            var bookId = string.Join(",", bookToReturn);

            var bookForReturn = _mapper.Map<IEnumerable<BookDto>>(mappedBook);

            var result =  CreatedAtRoute("GetBookCollection", 
                new { bookIds = bookId }, bookForReturn);

            return result;
        }
    }
}
