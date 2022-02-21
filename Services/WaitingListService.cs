using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;

namespace MeetingManagement.Services
{
    public class WaitingListService : IService<WaitingList>
    {
        #region Ctor
        private readonly ApplicationDbContext context;

        public WaitingListService(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        public void Add(WaitingList w)
        {
            context.WaitingLists.Add(w);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            WaitingList waitingList = context.WaitingLists.FirstOrDefault(w => w.WaitList_ID == Id);
            context.WaitingLists.Remove(waitingList);
            context.SaveChanges();
        }

        public List<WaitingList> GetAll()
        {
            return context.WaitingLists.ToList();
        }

        public WaitingList GetById(int Id)
        {
            return context.WaitingLists.FirstOrDefault(w => w.WaitList_ID == Id);
        }

        public void Update(WaitingList w, int Id)
        {
            WaitingList waitingList = context.WaitingLists.FirstOrDefault(w => w.WaitList_ID == Id);
            waitingList.WaitList_ID = w.WaitList_ID;
            waitingList.Book_ID = w.Book_ID;
            waitingList.Book = w.Book;
        }
    }
}
