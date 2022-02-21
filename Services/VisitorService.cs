using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;

namespace MeetingManagement.Services
{
    public class VisitorService : IService<Visitor>
    {
        #region Ctro
        private readonly ApplicationDbContext context;

        public VisitorService(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        public void Add(Visitor v)
        {
            context.Visitors.Add(v);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            Visitor visitor = context.Visitors.FirstOrDefault(v => v.Visitor_ID == Id);
            context.Visitors.Remove(visitor);
            context.SaveChanges();
        }

        public List<Visitor> GetAll()
        {
            return context.Visitors.ToList();
        }

        public Visitor GetById(int Id)
        {
            return context.Visitors.FirstOrDefault(v => v.Visitor_ID == Id);
        }

        public void Update(Visitor v, int Id)
        {
            Visitor visitor = context.Visitors.FirstOrDefault(v => v.Visitor_ID == Id);
            visitor.Visitor_ID = v.Visitor_ID;
            visitor.Name = v.Name;
            visitor.PhoneNumber = v.PhoneNumber;
            visitor.Email = v.Email;
            visitor.Nationality = v.Nationality;
            visitor.SSN = v.SSN;
            visitor.PhotoSSN = v.PhotoSSN;
            visitor.Employer = v.Employer;
            visitor.JobTitle = v.JobTitle;
            visitor.Vaccine = v.Vaccine;
            visitor.PhotoVaccine = v.PhotoVaccine;
            visitor.Book_ID = v.Book_ID;
            visitor.Book = v.Book;
            context.SaveChanges();
        }

        public List<Visitor> Search(string name)
        {
            return context.Visitors.Where(v => v.Name.Contains(name)).ToList();
        }
    }
}
