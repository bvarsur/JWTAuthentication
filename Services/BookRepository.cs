using JWTAuthentication.Models;
using System.Collections.Generic;
using System.Linq;

namespace JWTAuthentication.Services
{
    public class BookRepository : IBookRepository
    {
        private BookDbContext db;
        public BookRepository(BookDbContext context)
        {
            db = context;
        }
        public Book Add(Book book)
        {
            db.Books.Add(book);
            db.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
            return book;
        }

        public bool Delete(int Id)
        {
            var existBook = db.Books.Where(x => x.id == Id).FirstOrDefault();
            if (existBook != null)
            {
                db.Books.Remove(existBook);
                db.Entry(existBook).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                db.SaveChanges();
                return true;
            }
            else return false;
        }

        public Book GetBookById(int Id)
        {
            return db.Books.Where(x => x.id == Id).FirstOrDefault();
        }

        public List<Book> GetBooks()
        {
            return db.Books.ToList();
        }

        public Book Update(Book book)
        {
            var existBook = db.Books.Where(x => x.id == book.id).FirstOrDefault();
            if (existBook != null)
            {
                existBook.name = book.name;
                existBook.description = book.description;
                existBook.author = book.author;
                existBook.price = book.price;
                db.Books.Update(existBook);
                db.Entry(existBook).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                return existBook;
            }
            else return book;
        }
    }
    public interface IBookRepository
    {
        List<Book> GetBooks();
        Book GetBookById(int Id);
        Book Add(Book book);
        Book Update(Book book);
        bool Delete(int Id);
    }

}
