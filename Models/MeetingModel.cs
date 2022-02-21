using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingManagement.Models
{
    #region Book Status
    public enum BookStatus
    {
        Accepted,
        Waiting,
        Rejected
    }
    #endregion

    #region Application User
    [Table(name: "ApplicationUser")]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string UserId { get; set; }
        public virtual List<Book> Books { get; set; }
    }
    #endregion

    #region Users
    [Table(name: "Users")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        public string? UserName { get; set; }
        public string? Password { get; set; }
        [MaxLength(255)]
        public string? FullName { get; set; }
        [MaxLength(255)]
        public string? Photo { get; set; }
        [MaxLength(255)]
        public string? Email { get; set; }
        public float? CreatedUserId { get; set; }
        public float? UpdatedUserId { get; set; }
        [MaxLength(255)]
        public string? CreatedDate { get; set; }
        [MaxLength(255)]
        public string? UpdatedDate { get; set; }
        public int Active { get; set; }
        public int? RoleId { get; set; }
        [MaxLength(255)]
        public string? Title { get; set; }
        [MaxLength(255)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? Phone { get; set; }
        public DateTime? LastLogin { get; set; }


    }
    #endregion

    #region Building
    [Table(name: "Building")]
    public class Building
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Building_ID { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Image { get; set; }

        [Required, MaxLength(100)]
        public string Location { get; set; }
        public virtual List<ConferenceRoom> ConferenceRooms { get; set; }
    }
    #endregion

    #region Conference Room
    [Table(name: "ConferenceRoom")]
    public class ConferenceRoom
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConferenceRoom_ID { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Image { get; set; }

        [Required]
        public int Capacity { get; set; }
        public int Building_ID { get; set; }

        [ForeignKey("Building_ID")]
        public virtual Building Building { get; set; }
        public virtual List<Book> Books { get; set; }
        public virtual List<AdditionsConferenceRoom> AdditionsConferenceRooms { get; set; }
    }
    #endregion

    #region Book
    [Table(name: "Book")]
    public class Book
    {
        public Book()
        {
            CreatedAt = DateTime.Now;
            BookStatus = BookStatus.Waiting;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Book_ID { get; set; }

        [Required]
        public string UserID { get; set; }

        public int ConferenceRoom_ID { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public BookStatus BookStatus { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Required]
        public int NumOfVisitors { get; set; }

        [Required]
        public int InternalVisitor { get; set; }

        [Required]
        public int ExternalVisitor { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ConferenceRoom_ID")]
        public virtual ConferenceRoom ConferenceRoom { get; set; }
        public virtual List<Visitor> Visitors { get; set; }
        public virtual List<WaitingList> WaitingLists { get; set; }
        public virtual List<AdditionsBook> AdditionsBooks { get; set; }
    }
    #endregion

    #region Visitors
    [Table(name: "Visitors")]
    public class Visitor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Visitor_ID { get; set; }
        public int Book_ID { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [Phone, DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; }
        public string? SSN { get; set; }
        public string? PhotoSSN { get; set; }

        [EmailAddress, DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Employer { get; set; }
        public string? JobTitle { get; set; }
        public string? Vaccine { get; set; }
        public string? PhotoVaccine { get; set; }

        [ForeignKey("Book_ID")]
        public virtual Book Book { get; set; }
    }
    #endregion

    #region Waiting List
    [Table(name: "WaitingList")]
    public class WaitingList
    {
        public WaitingList()
        {
            CreatedAt = DateTime.Now;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WaitList_ID { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public int Book_ID { get; set; }

        [ForeignKey("Book_ID")]
        public virtual Book Book { get; set; }
    }
    #endregion

    #region Additions
    [Table(name: "Additions")]
    public class Additions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Additions_ID { get; set; }

        [Required]
        public string Name { get; set; }
        public virtual List<AdditionsBook> AdditionsBooks { get; set; }
        public virtual List<AdditionsConferenceRoom> AdditionsConferenceRooms { get; set; }
    }
    #endregion

    #region Additions Book
    [Table(name: "AdditionsBook")]
    public class AdditionsBook
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdditionsBook_ID { get; set; }
        public int Additions_ID { get; set; }
        public int Book_ID { get; set; }

        [ForeignKey("Book_ID")]
        public virtual Book Book { get; set; }

        [ForeignKey("Additions_ID")]
        public virtual Additions Additions { get; set; }
    }
    #endregion

    #region Additions Conference Room
    [Table(name: "AdditionsConferenceRoom")]
    public class AdditionsConferenceRoom
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdditionsConferenceRoom_ID { get; set; }
        public int Additions_ID { get; set; }
        public int ConferenceRoom_ID { get; set; }

        [ForeignKey("ConferenceRoom_ID")]
        public virtual ConferenceRoom ConferenceRoom { get; set; }

        [ForeignKey("Additions_ID")]
        public virtual Additions Additions { get; set; }
    }
    #endregion
}
