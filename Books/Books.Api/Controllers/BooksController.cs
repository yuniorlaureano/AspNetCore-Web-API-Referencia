using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Api.Filters;
using Books.Api.Models;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Books.Api.Entities;

namespace Books.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBookRepository _bookRepository;
        private IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet("{bookId}", Name = "GetBook")]
        [BookResultFilter]
        public async Task<IActionResult> GetBook(Guid bookId)
        {
            var bookEntity = await _bookRepository.GetBookAsync(bookId);

            if (bookEntity == null)
            {
                return NotFound();
            }

            return Ok(bookEntity);
        }

        [HttpGet(Name = "GetBooks")]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            var bookEntities = await _bookRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        [HttpPost(Name = "CreateBook")]
        [BookResultFilter]
        public async Task<IActionResult> CreateBook([FromBody] BookForCreationDto bookDto)
        {
            var bookEntity = _mapper.Map<Book>(bookDto);

            _bookRepository.AddBook(bookEntity);

            await _bookRepository.SaveChangesAsync();

            return CreatedAtRoute("GetBook" , 
                new { bookId = bookEntity.Id }, 
                bookEntity);
        }
    }
}  