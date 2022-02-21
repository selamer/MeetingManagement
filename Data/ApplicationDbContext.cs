using MeetingManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetingManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<ConferenceRoom> ConferenceRooms { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Visitor> Visitors { get; set; }
        public virtual DbSet<WaitingList> WaitingLists { get; set; }
        public virtual DbSet<Additions> Additions { get; set; }
        public virtual DbSet<AdditionsBook> AdditionsBooks { get; set; }
        public virtual DbSet<AdditionsConferenceRoom> AdditionsConferenceRooms { get; set; }
    }
}