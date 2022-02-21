using MeetingManagement.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace MeetingManagement.Controllers
{
    public class HomeController : Controller
    {
        #region Ctor
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<SharedResource> localizer;

        public HomeController(ILogger<HomeController> logger,
                              IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            this.localizer = localizer;
        }
        #endregion

        #region Set Language
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddMonths(1) });
            return LocalRedirect(returnUrl);
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = localizer["Privacy Policy"];
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = localizer["Contact"];
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = localizer["About"];
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}