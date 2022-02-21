using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingManagement.Services
{
    public class BookService : IService<Book>
    {
        #region Ctor
        private readonly ApplicationDbContext context;

        public BookService(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        public void Add(Book b)
        {
            context.Books.Add(b);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            Book book = context.Books.Include(b => b.ConferenceRoom).Include(b => b.User).Include(b => b.AdditionsBooks).FirstOrDefault(b => b.Book_ID == Id);
            context.Books.Remove(book);
            context.SaveChanges();
        }

        public List<Book> GetAll()
        {
            List<Book> books = context.Books.Include(b => b.ConferenceRoom).Include(b => b.AdditionsBooks).Include(b => b.User).ToList();
            return books;
        }

        public Book GetById(int Id)
        {
            return context.Books.Include(b => b.ConferenceRoom).Include(b => b.AdditionsBooks).Include(b => b.User).Include(b => b.Visitors).FirstOrDefault(b => b.Book_ID == Id);
        }

        public void Update(Book b, int Id)
        {
            Book book = context.Books.Include(b => b.ConferenceRoom).Include(b => b.AdditionsBooks).Include(b => b.AdditionsBooks).Include(b => b.User).FirstOrDefault(b => b.Book_ID == Id);
            book.Book_ID = b.Book_ID;
            book.UserID = b.UserID;
            book.User = b.User;
            book.ConferenceRoom_ID = b.ConferenceRoom_ID;
            book.ConferenceRoom = b.ConferenceRoom;
            book.CreatedAt = b.CreatedAt;
            book.BookStatus = b.BookStatus;
            book.Date = b.Date;
            book.StartTime = b.StartTime;
            book.EndTime = b.EndTime;
            book.NumOfVisitors = b.NumOfVisitors;
            book.InternalVisitor = b.InternalVisitor;
            book.ExternalVisitor = b.ExternalVisitor;
            book.WaitingLists = b.WaitingLists;
            book.AdditionsBooks = b.AdditionsBooks;
            context.SaveChanges();
        }
    }
}