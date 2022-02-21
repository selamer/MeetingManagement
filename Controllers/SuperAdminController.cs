using MeetingManagement.Data;
using MeetingManagement.Interfaces;
using MeetingManagement.Models;
using MeetingManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MeetingManagement.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class SuperAdminController : Controller
    {
        #region Ctor
        private readonly IService<Building> buildingService;
        private readonly ApplicationDbContext context;
        private readonly IService<Book> bookService;
        private readonly IService<Additions> additionsService;
        private readonly IService<ConferenceRoom> conferenceRoomService;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IStringLocalizer<SharedResource> localizer;

        public SuperAdminController(IService<Building> BuildingService,
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

        #region Upload Image
        private string ImageBuilding(BuildingViewModel model)
        {
            string uniqueFileName = null;
            if (model.PhotoFile != null)
            {
                string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images/Categories");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PhotoFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string ImageRoom(RoomViewModel model)
        {
            string uniqueFileName = null;
            if (model.PhotoFile != null)
            {
                string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PhotoFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        #endregion

        #region Index Super Admin
        public IActionResult Index()
        {
            ViewData["Title"] = localizer["Super Admin Page"];
            return View();
        }
        #endregion

        #region Buildings
        public IActionResult getBuilding()
        {
            ViewData["Title"] = localizer["All Buildings"];
            return View(buildingService.GetAll());
        }

        [HttpGet]
        public IActionResult EditBuilding(int Id)
        {
            ViewData["Title"] = localizer["Edit Building"];
            Building building = buildingService.GetById(Id);
            BuildingViewModel model =
                new BuildingViewModel
                {
                    Building_ID = building.Building_ID,
                    Name = building.Name,
                    Location = building.Location,
                    Image = building.Image,
                };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBuilding(BuildingViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Image = ImageBuilding(model);
                Building building =
                    new Building
                    {
                        Building_ID = model.Building_ID,
                        Name = model.Name,
                        Location = model.Location,
                        Image = model.Image
                    };
                //if (model.PhotoFile != null)
                //{
                //    if (model.Image != null)
                //    {
                //        string filePath = Path.Combine(hostEnvironment.WebRootPath, "images/Categories", model.Image);
                //        System.IO.File.Delete(filePath);
                //    }
                //    building.Image = ImageBuilding(model);
                //}
                buildingService.Update(building, building.Building_ID);
                return RedirectToAction("getBuilding");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddBuilding()
        {
            ViewData["Title"] = localizer["Add Building"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBuilding(BuildingViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Image = ImageBuilding(model);
                Building building =
                    new Building
                    {
                        Name = model.Name,
                        Location = model.Location,
                        Image = model.Image
                    };
                buildingService.Add(building);
                return RedirectToAction("getBuilding");
            }
            return View(model);
        }

        public IActionResult DeleteBuilding(int Id)
        {
            buildingService.Delete(Id);
            return RedirectToAction("getBuilding");
        }
        #endregion

        #region Conference Rooms
        public IActionResult getRoom()
        {
            ViewData["Title"] = localizer["All Rooms"];
            return View(conferenceRoomService.GetAll());
        }

        [HttpGet]
        public IActionResult EditRoom(int Id)
        {
            ViewData["Title"] = localizer["Edit Room"];
            ViewBag.Building = buildingService.GetAll();
            ConferenceRoom conferenceRoom = conferenceRoomService.GetById(Id);
            RoomViewModel model =
                new RoomViewModel
                {
                    ConferenceRoom_ID = conferenceRoom.ConferenceRoom_ID,
                    AdditionsConferenceRooms = conferenceRoom.AdditionsConferenceRooms,
                    Building_ID = conferenceRoom.Building_ID,
                    Capacity = conferenceRoom.Capacity,
                    Image = conferenceRoom.Image,
                    Name = conferenceRoom.Name
                };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoom(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Building = buildingService.GetAll();
                model.Image = ImageRoom(model);
                ConferenceRoom room =
                    new ConferenceRoom
                    {
                        ConferenceRoom_ID = model.ConferenceRoom_ID,
                        Name = model.Name,
                        Building_ID = model.Building_ID,
                        Capacity = model.Capacity,
                        Image = model.Image,
                        AdditionsConferenceRooms = model.AdditionsConferenceRooms,
                    };
                conferenceRoomService.Update(room, room.ConferenceRoom_ID);
                return RedirectToAction("getRoom");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddRoom()
        {
            ViewData["Title"] = localizer["Add Room"];
            ViewBag.Building = buildingService.GetAll();
            ViewBag.Additions = additionsService.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.Additions)
                {
                    model.AdditionsConferenceRooms.Add(new AdditionsConferenceRoom { Additions_ID = item });
                }
                model.Image = ImageRoom(model);
                ConferenceRoom conference =
                    new ConferenceRoom
                    {
                        AdditionsConferenceRooms = model.AdditionsConferenceRooms,
                        Building_ID = model.Building_ID,
                        Capacity = model.Capacity,
                        Image = model.Image,
                        Name = model.Name,
                        ConferenceRoom_ID = model.ConferenceRoom_ID
                    };
                conferenceRoomService.Add(conference);
                return RedirectToAction("getRoom");
            }
            return View(model);
        }

        public IActionResult DeleteRoom(int Id)
        {
            conferenceRoomService.Delete(Id);
            return RedirectToAction("getRoom");
        }
        #endregion
    }
}

