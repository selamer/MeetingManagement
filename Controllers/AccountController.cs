using MeetingManagement.Data;
using MeetingManagement.Models;
using MeetingManagement.Services;
using MeetingManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.DirectoryServices;

namespace MeetingManagement.Controllers
{
    public class AccountController : Controller
    {
        #region Ctor
        private readonly AccountService account;
        private readonly ApplicationDbContext context;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(AccountService account,
                                 ApplicationDbContext context,
                                 IStringLocalizer<SharedResource> localizer,
                                 SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager)
        {
            this.account = account;
            this.localizer = localizer;
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        #endregion

        #region Register Action
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["Title"] = localizer["Register"];
            return View();
        }

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    bool Succeeded = await account.Register(model);
        //    if (Succeeded)
        //    {
        //        return RedirectToAction("Index", "Building");
        //    }
        //    else
        //    {
        //        return View(model);
        //    }
        //}

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = ProcessLogin(model.UserName, model.Password);
                    if (user)
                    {
                        ApplicationUser users = new ApplicationUser { UserId = model.UserId, UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                        var Result = await userManager.CreateAsync(users, model.Password);
                        if (Result.Succeeded)
                        {
                            await signInManager.SignInAsync(users, isPersistent: false);
                            return RedirectToAction("Index", "Building");
                        }
                        else
                        {
                            ModelState.AddModelError("Username", "Incorrect Username or Password");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Username", "Incorrect Username or Password");
                        ViewBag.error = "Incorrect Username or Password";
                        return View(model);
                    }
                }
                else
                {
                    string str = "";
                    foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            str += error.ErrorMessage + Environment.NewLine;
                        }
                    }
                }
                return RedirectToAction("Register", "Account");
            }
            catch (Exception)
            {
                return RedirectToAction("Register", "Account");
            }
        }
        #endregion

        #region Login Action
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = @localizer["Login"];
            return View();
        }

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    bool Succeded = await account.Login(model);
        //    if (Succeded)
        //    {
        //        return RedirectToAction("Index", "Building");
        //    }
        //    ModelState.AddModelError(string.Empty, localizer["InValid Login Attempt..!"]);
        //    return View(model);
        //}

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = ProcessLogin(model.UserName, model.Password);
                    if (user)
                    {
                        var Result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
                        if (Result.Succeeded)
                        {
                            return RedirectToAction("Index", "Building");
                        }
                        else
                        {
                            ModelState.AddModelError("Username", "Incorrect Username or Password");
                            ViewBag.error = "Incorrect Username or Password";
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Username", "Incorrect Username or Password");
                        ViewBag.error = "Incorrect Username or Password";
                        return View(model);
                    }
                }
                else
                {
                    string str = "";
                    foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            str += error.ErrorMessage + Environment.NewLine;
                        }
                    }
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region Active Directory Authenticated
        private bool ProcessLogin(string username, string password)
        {
            if (username != null && password != null)
            {
                bool a = IsAuthenticated("LDAP://mcit.core.local", username, password);
                if (a == true)
                    return true;
            }
            return false;
        }

        protected bool IsAuthenticated(string srvr, string usr, string pwd)
        {
            bool authenticated = false;
            object obj = null;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(srvr, @"mcit\" + usr, pwd);
                obj = entry.NativeObject;
                authenticated = true;
            }
            catch (DirectoryServicesCOMException) { }
            catch (Exception) { }
            return authenticated;
        }
        #endregion

        #region Logout Action
        public IActionResult Logout()
        {
            ViewData["Title"] = localizer["Logout"];
            account.Logout();
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Access Denied 
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewData["Title"] = localizer["Access Denied"];
            return View();
        }
        #endregion
    }
}
