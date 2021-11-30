using JWTAuthentication.Models;
using JWTAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Route("Books")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private IBookRepository bookRepository;
        public BookController(IBookRepository repository)
        {
            bookRepository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<Book> GetBooks()
        {
            return bookRepository.GetBooks();
        }

        [HttpGet("{Id}")] 
        public Book GetBookById([FromRoute] int Id)
        {
            if (Id == 0)
                throw new Exception("Please provide Id.");
            return bookRepository.GetBookById(Id: Id);
        }

        [HttpPost]
        public Book Add([FromBody] Book book)
        {
            if (book == null)
                throw new Exception("Please provide details.");
            return bookRepository.Add(book: book);
        }

        [HttpPut]
        public Book Update([FromBody] Book book)
        {
            if (book == null)
                throw new Exception("Please provide details.");
            if (book.price == 0)
                throw new Exception("Book price should not have zero amount.");

            var exist = bookRepository.GetBookById(Id: book.id);
            if (exist == null)
                throw new Exception("Please provide valid book Id.");

            return bookRepository.Update(book: book);
        }

        [HttpDelete]
        [Route("{Id}")]
        public bool Delete([FromRoute] int Id)
        {
            if (Id == 0)
                throw new Exception("Please provide Id.");
            return bookRepository.Delete(Id);
        }
    }
}
