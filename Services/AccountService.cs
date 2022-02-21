using MeetingManagement.Data;
using MeetingManagement.Models;
using MeetingManagement.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace MeetingManagement.Services
{
    public class AccountService
    {
        #region Ctor
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        #endregion

        #region Register Service
        public async Task<bool> Register(RegisterViewModel model)
        {
            ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.UserName, PhoneNumber = model.PhoneNumber, UserId = model.UserId };
            var Result = await userManager.CreateAsync(user, model.Password);
            if (Result.Succeeded)
            {
                signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Login Service
        public async Task<bool> Login(LoginViewModel model)
        {
            var Result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (Result.Succeeded)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Logout Service
        public void Logout()
        {
            signInManager.SignOutAsync();
        }
        #endregion
    }
}
