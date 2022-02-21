using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingManagement.Services
{
    public class ConferenceRoomServive : IService<ConferenceRoom>
    {
        #region Ctor
        private readonly ApplicationDbContext context;

        public ConferenceRoomServive(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        public void Add(ConferenceRoom c)
        {
            context.ConferenceRooms.Add(c);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            ConferenceRoom conferenceRoom = context.ConferenceRooms.Include(c => c.Books).ThenInclude(c => c.User).Include(c => c.AdditionsConferenceRooms).FirstOrDefault(c => c.ConferenceRoom_ID == Id);
            context.ConferenceRooms.Remove(conferenceRoom);
            context.SaveChanges();
        }

        public List<ConferenceRoom> GetAll()
        {
            return context.ConferenceRooms.Include(c => c.Building).Include(c => c.Books).ThenInclude(c => c.User).ToList();
        }

        public ConferenceRoom GetById(int Id)
        {
            return context.ConferenceRooms.Include(c => c.Books).ThenInclude(c => c.User).Include(c => c.AdditionsConferenceRooms).FirstOrDefault(c => c.ConferenceRoom_ID == Id);
        }

        public void Update(ConferenceRoom c, int Id)
        {
            ConferenceRoom conferenceRoom = context.ConferenceRooms.Include(c => c.Books).ThenInclude(c => c.User).Include(c => c.AdditionsConferenceRooms).FirstOrDefault(c => c.ConferenceRoom_ID == Id);
            conferenceRoom.ConferenceRoom_ID = c.ConferenceRoom_ID;
            conferenceRoom.Name = c.Name;
            conferenceRoom.Capacity = c.Capacity;
            conferenceRoom.Image = c.Image;
            conferenceRoom.Building_ID = c.Building_ID;
            conferenceRoom.Building = c.Building;
            conferenceRoom.Books = c.Books;
            conferenceRoom.Image = c.Image;
            conferenceRoom.AdditionsConferenceRooms = c.AdditionsConferenceRooms;
            context.SaveChanges();
        }

        public List<ConferenceRoom> Search(string name)
        {
            return context.ConferenceRooms.Where(b => b.Name.Contains(name)).ToList();
        }
    }
}