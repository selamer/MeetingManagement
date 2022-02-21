using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MeetingManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        #region Ctor
        private readonly IService<Building> buildingService;
        private readonly ApplicationDbContext context;
        private readonly IService<Book> bookService;
        private readonly IService<Additions> additionsService;
        private readonly IService<ConferenceRoom> conferenceRoomService;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IStringLocalizer<SharedResource> localizer;

        public AdminController(IService<Building> BuildingService,
                               ApplicationDbContext context,
                               IService<Book> BookService,
                               IService<Additions> AdditionsService,
                               IService<ConferenceRoom> ConferenceRoomService,
                               IWebHostEnvironment hostEnvironment,
                               IStringLocalizer<SharedResource> localizer)
        {
            buildingService = BuildingService;
            this.context = context;
            bookService = BookService;
            additionsService = AdditionsService;
            conferenceRoomService = ConferenceRoomService;
            this.hostEnvironment = hostEnvironment;
            this.localizer = localizer;
        }
        #endregion

        public IActionResult Index()
        {
            ViewData["Title"] = localizer["Admin Page"];
            return View();
        }

        public IActionResult WaitingBooks()
        {
            ViewData["Title"] = localizer["Waiting Books"];
            List<Book> books = bookService.GetAll().Where(b => (b.Date.Date >= DateTime.Now.Date) && (b.BookStatus == BookStatus.Waiting))
                .OrderByDescending(b => b.Date.Date).OrderByDescending(b => b.StartTime).ToList();
            ViewBag.Message = TempData["Message"];
            return View(books);
        }

        public IActionResult AcceptedBooks()
        {
            ViewData["Title"] = localizer["Accepted Books"];
            List<Book> books = bookService.GetAll().Where(b => (b.Date.Date >= DateTime.Now.Date) && (b.BookStatus == BookStatus.Accepted))
                .OrderByDescending(b => b.Date.Date).OrderByDescending(b => b.StartTime).ToList();
            return View(books);
        }

        public IActionResult RejectedBooks()
        {
            ViewData["Title"] = localizer["Rejected Books"];
            List<Book> books = bookService.GetAll().Where(b => (b.Date.Date >= DateTime.Now.Date) && (b.BookStatus == BookStatus.Rejected))
                .OrderByDescending(b => b.Date.Date).OrderByDescending(b => b.StartTime).ToList();
            return View(books);
        }

        public IActionResult AcceptBook(int Id)
        {
            Book b = bookService.GetById(Id);
            var books = bookService.GetAll();
            var status = books.Where(br => br.BookStatus == BookStatus.Accepted && br.Date == b.Date).Count();
            if (status != 0)
            {
                var result = books.Where(br =>
                (br.Date == b.Date && br.StartTime.Hour == b.StartTime.Hour) || br.Date == b.Date &&
                (br.EndTime.Hour > b.StartTime.Hour && br.EndTime.Minute > b.StartTime.Minute)).Count();

                if (result != 0)
                {
                    TempData["Message"] = localizer["The Appoiment Is Booked..!!"];
                    return RedirectToAction("WaitingBooks");
                }
            }
            b.BookStatus = BookStatus.Accepted;
            bookService.Update(b, Id);
            SendEmail(b);
            return RedirectToAction("WaitingBooks");
        }

        public IActionResult RejectBook(int Id)
        {
            Book b = bookService.GetById(Id);
            b.BookStatus = BookStatus.Rejected;
            bookService.Update(b, Id);
            return RedirectToAction("WaitingBooks");
        }

        public IActionResult DeleteBook(int Id)
        {
            bookService.Delete(Id);
            return RedirectToAction("getBook");
        }

        public void SendEmail(Book b)
        {
            StringBuilder MailToEmployee = new StringBuilder();
            MailToEmployee.Append("<div dir='rtl' style='margin: auto; width: 50%;'>");
            MailToEmployee.AppendLine("<div style='text-align: center; padding: 1rem 1rem; padding: 1rem 1rem; border: 1px solid rgba(0, 0, 0, 0.125); border-radius: 0.25rem; max-width: 500px;'> نحيطكم علما بأنه تم قبول حجز القاعه فى المعياد المختار من قبلكم ");
            MailToEmployee.AppendLine("</div>");
            MailToEmployee.Append("<div dir='rtl' style='margin: auto; width: 50%;'>");
            MailToEmployee.AppendLine("<div style='text-align: center; padding: 1rem 1rem; padding: 1rem 1rem; border: 1px solid rgba(0, 0, 0, 0.125); border-radius: 0.25rem; max-width: 500px;'> معاد الحجز ");
            MailToEmployee.AppendLine("<br /> اسم القاعه : " + b.ConferenceRoom.Name + "<br /> تاريخ الحجز : " + b.Date + "<br /> بداية الحجز : " + b.StartTime + "<br /> نهاية الحجز : " + b.EndTime + "<br /> عدد الزائرين : " + b.NumOfVisitors);
            MailToEmployee.AppendLine("</div>");
            MailToEmployee.AppendLine("<br /> <p> في حالة الإحتياج إلى أى طلبات من الكافتريا، يرجي الإتصال على الرقم الخاص بها ..(5555) </p>");
            MailToEmployee.AppendLine("<br /> <p> في حالة وجود أى أستفسار بخصوص محتوى الإيميل، يرجى التواصل على الرقم الداخلى لدعم تطبيق حجز قاعة مؤتمرات ..(5555) </p>");
            MailToEmployee.AppendLine("</div>");

            // Send Email to employee
            try
            {
                MailMessage Message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                Message.From = new MailAddress("user_profile@mcit.gov.eg");
                Message.To.Add(new MailAddress(b.User.Email));
                Message.Subject = "تأكيد حجز قاعة المؤتمرات";
                Message.IsBodyHtml = true; //to make message body as html  
                Message.Body = MailToEmployee.ToString();
                smtp.Port = 25;
                smtp.Host = "10.210.23.51"; //for email host  
                smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("user_profile@mcit.gov.eg", "P@ssw0rd_mcit2021");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(Message);
            }
            catch (Exception e)
            {
                var ss = e;
                ViewBag.sds = ss;
            }
        }
    }
}