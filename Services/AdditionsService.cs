using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;

namespace MeetingManagement.Services
{

    public class AdditionsService : IService<Additions>
    {
        #region Ctor
        private readonly ApplicationDbContext context;

        public AdditionsService(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        public void Add(Additions a)
        {
            context.Additions.Add(a);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            Additions additions = context.Additions.FirstOrDefault(b => b.Additions_ID == Id);
            context.Additions.Remove(additions);
            context.SaveChanges();
        }

        public List<Additions> GetAll()
        {
            return context.Additions.ToList();
        }

        public Additions GetById(int Id)
        {
            return context.Additions.FirstOrDefault(a => a.Additions_ID == Id);
        }

        public void Update(Additions a, int Id)
        {
            Additions additions = context.Additions.FirstOrDefault(a => a.Additions_ID == Id);
            additions.Additions_ID = a.Additions_ID;
            additions.Name = a.Name;
            additions.AdditionsConferenceRooms = a.AdditionsConferenceRooms;
            additions.AdditionsBooks = a.AdditionsBooks;
            context.SaveChanges();
        }
    }
}