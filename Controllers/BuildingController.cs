using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MeetingManagement.Controllers
{
    public class BuildingController : Controller
    {
        #region Ctor
        private readonly IService<Building> buildingService;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ApplicationDbContext context;

        public BuildingController(IService<Building> BuildingService,
                                  IStringLocalizer<SharedResource> localizer,
                                  ApplicationDbContext context)
        {
            buildingService = BuildingService;
            this.localizer = localizer;
            this.context = context;
        }
        #endregion

        #region All Building "Home"
        public IActionResult Index()
        {
            ViewData["Title"] = localizer["Home"];
            return View(buildingService.GetAll());
        }
        #endregion

        #region Building Search
        public IActionResult BuildingSearch(string name)
        {
            List<Building> buildings = context.Buildings.Where(b => b.Name.Contains(name)).ToList();
            return View("Index", buildings);
        }
        #endregion
    }
}
