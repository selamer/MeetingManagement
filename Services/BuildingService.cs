using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingManagement.Services
{
    public class BuildingService : IService<Building>
    {
        #region Ctor
        private readonly ApplicationDbContext context;

        public BuildingService(ApplicationDbContext context)
        {
            this.context = context;
        }
        #endregion

        public void Add(Building b)
        {
            context.Buildings.Add(b);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            Building building = context.Buildings.FirstOrDefault(b => b.Building_ID == Id);
            context.Buildings.Remove(building);
            context.SaveChanges();
        }

        public List<Building> GetAll()
        {
            return context.Buildings.Include(b => b.ConferenceRooms).ToList();
        }

        public Building GetById(int Id)
        {
            return context.Buildings.Include(b => b.ConferenceRooms).FirstOrDefault(b => b.Building_ID == Id);
        }

        public void Update(Building b, int Id)
        {
            Building building = context.Buildings.FirstOrDefault(b => b.Building_ID == Id);
            building.Building_ID = b.Building_ID;
            building.Name = b.Name;
            building.Location = b.Location;
            building.Image = b.Image;
            context.SaveChanges();
        }
    }
}