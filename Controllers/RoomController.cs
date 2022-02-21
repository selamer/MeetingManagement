using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;
using MeetingManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using X.PagedList;

namespace MeetingManagement.Controllers
{
    public class RoomController : Controller
    {
        #region Ctor
        private readonly IService<ConferenceRoom> conferenceRoomService;
        private readonly IService<Book> bookService;
        private readonly IService<Building> buildingService;
        private readonly IService<Additions> additionsService;
        private readonly IService<Visitor> visitorService;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStringLocalizer<SharedResource> localizer;

        public RoomController(IService<ConferenceRoom> ConferenceRoomService,
                              IService<Book> BookService,
                              IService<Building> buildingService,
                              IService<Additions> AdditionsService,
                              IService<Visitor> VisitorService,
                              IWebHostEnvironment hostEnvironment,
                              ApplicationDbContext context,
                              UserManager<ApplicationUser> userManager,
                              IStringLocalizer<SharedResource> localizer)
        {
            conferenceRoomService = ConferenceRoomService;
            bookService = BookService;
            this.buildingService = buildingService;
            additionsService = AdditionsService;
            visitorService = VisitorService;
            this.hostEnvironment = hostEnvironment;
            this.context = context;
            this.userManager = userManager;
            this.localizer = localizer;
        }
        #endregion

        #region Upload Photo
        private string VisitorPhotoSSN(VisitorViewModel model)
        {
            string uniqueFileName = null;
            if (model.ImageSSN != null)
            {
                string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images/VisitorPhotoSSN");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageSSN.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageSSN.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string VisitorPhotoVaccine(VisitorViewModel model)
        {
            string uniqueFileName = null;
            if (model.ImageVaccine != null)
            {
                string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images/VisitorPhotoSSN");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageVaccine.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageVaccine.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        #endregion

        #region All Conference Room
        public IActionResult Index(int Id)
        {
            ViewData["Title"] = localizer["Conference Rooms"];
            List<ConferenceRoom> conferenceRooms = conferenceRoomService.GetAll().Where(c => c.Building_ID == Id).ToList();
            ViewBag.MinisterRoom = conferenceRoomService.GetAll().Where(c => c.ConferenceRoom_ID == 4);
            return View("Index", conferenceRooms);
        }
        #endregion

        #region Conference Room Details
        public IActionResult Details(int Id)
        {
            ViewData["Title"] = localizer["Room Details"];
            List<Book> books = bookService.GetAll().Where(b => b.ConferenceRoom_ID == Id)
                .Where(b => b.Date.Date >= DateTime.Now.Date).OrderByDescending(b => b.Date).ToList();
            ViewBag.ConferenceRoom = conferenceRoomService.GetAll().Where(c => c.ConferenceRoom_ID == Id);
            ViewBag.BookWiating = bookService.GetAll().Where(b => b.ConferenceRoom_ID == Id && b.Date.Date >= DateTime.Now.Date && b.BookStatus == BookStatus.Waiting).Count();
            ViewBag.BookAccepted = bookService.GetAll().Where(b => b.ConferenceRoom_ID == Id && b.Date.Date >= DateTime.Now.Date && b.BookStatus == BookStatus.Accepted).Count();
            return View("Details", books);
        }
        #endregion

        #region Book Meeting
        [HttpGet]
        public IActionResult BookRoom(int Id)
        {
            ViewData["Title"] = localizer["Book Conference Room"];
            List<AdditionsConferenceRoom> additionsConferenceRooms =
                conferenceRoomService.GetById(Id).AdditionsConferenceRooms.Where(a => a.ConferenceRoom_ID == Id).ToList();
            List<Additions> additions = new List<Additions>();
            foreach (var item in additionsConferenceRooms)
            {
                additions.Add(additionsService.GetById(item.Additions_ID));
            }
            ViewBag.Additions = additions;
            TempData["Capacity"] = conferenceRoomService.GetById(Id);
            return View(new BookViewModel { ConferenceRoom_ID = Id });
        }


        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookRoom(BookViewModel b)
        {
            if (ModelState.IsValid)
            {
                if (b.ExternalVisitors > b.NumOfVisitors || b.StartTime.CompareTo(b.EndTime) > b.EndTime.CompareTo(b.StartTime))
                {
                    ModelState.AddModelError("", localizer["Data InValid..!"]);
                    return View(b);
                }
                ApplicationUser user = await userManager.FindByNameAsync(User.Identity.Name);
                Book book = new Book
                {
                    ConferenceRoom_ID = b.ConferenceRoom_ID,
                    CreatedAt = DateTime.Now,
                    Date = b.Date,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    NumOfVisitors = b.NumOfVisitors,
                    ExternalVisitor = b.ExternalVisitors,
                    InternalVisitor = b.NumOfVisitors - b.ExternalVisitors,
                    UserID = user.Id
                };
                bookService.Add(book);
                int bookId = bookService.GetAll().OrderByDescending(b => b.Book_ID).FirstOrDefault().Book_ID;
                if (b.Additions.Count > 0)
                {
                    for (int i = 0; i < b.Additions.Count; i++)
                    {
                        AdditionsBook additionsBook = new AdditionsBook { Additions_ID = b.Additions[i], Book_ID = bookId };
                        context.AdditionsBooks.Add(additionsBook);
                    }
                    context.SaveChanges();
                }
                if (book.ExternalVisitor > 0)
                {
                    return RedirectToAction("ExVisitors", new { Id = book.Book_ID });
                }
            }
            else
            {
                ModelState.AddModelError("", ModelState.ErrorCount.ToString());
                return RedirectToAction("Details", new { Id = b.ConferenceRoom_ID });
            }
            return RedirectToAction("Details", new { Id = b.ConferenceRoom_ID });
        }
        #endregion

        #region Add Visitors Data
        public IActionResult ExVisitors(int? page, int Id)
        {
            ViewData["Title"] = localizer["Visitors Data"];
            var pageNumber = page ?? 1;
            var pageSize = 1;

            Book book = bookService.GetById(Id);
            ViewBag.BookId = book.Book_ID;
            ViewBag.Room_Id = book.ConferenceRoom_ID;
            List<VisitorViewModel> visitors;
            if (book.Visitors.Count == 0)
            {
                VisitorViewModel[] model = new VisitorViewModel[book.ExternalVisitor];
                visitors = new List<VisitorViewModel>(model);
                ViewBag.VisitorNum = localizer["Data Of A Visitor Number  1"];
            }
            else
            {
                VisitorViewModel[] model = new VisitorViewModel[book.ExternalVisitor - book.Visitors.Count];
                if (model.Length == 0)
                {
                    return RedirectToAction("Details", new { Id = book.ConferenceRoom_ID });
                }
                else
                {
                    visitors = new List<VisitorViewModel>(model);
                    ViewBag.VisitorNum = localizer["Data Of A Visitor Number"] + " " + (book.Visitors.Count() + 1);
                }
            }
            return View(visitors.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public IActionResult AddVisitor(VisitorViewModel model)
        {
            Book book = bookService.GetById(model.Book_Id);

            if (model.ImageSSN != null || model.ImageVaccine != null)
            {
                if (model.ImageSSN != null)
                {
                    model.PhotoSSN = VisitorPhotoSSN(model);
                }
                if (model.ImageVaccine != null)
                {
                    model.PhotoVaccine = VisitorPhotoVaccine(model);
                }
                visitorService.Add(new Visitor
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    SSN = model.SSN,
                    PhotoSSN = model.PhotoSSN,
                    Email = model.Email,
                    Employer = model.Employer,
                    JobTitle = model.JobTitle,
                    Nationality = model.Nationality,
                    Vaccine = model.Vaccine,
                    PhotoVaccine = model.PhotoVaccine,
                    Book_ID = model.Book_Id
                });
            }
            else
            {
                visitorService.Add(new Visitor
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    SSN = model.SSN,
                    Email = model.Email,
                    Employer = model.Employer,
                    JobTitle = model.JobTitle,
                    Nationality = model.Nationality,
                    Vaccine = model.Vaccine,
                    Book_ID = model.Book_Id
                });
            }
            return RedirectToAction("ExVisitors", new { Id = book.Book_ID });
        }
        #endregion
    }
}
