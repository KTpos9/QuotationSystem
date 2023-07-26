using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Configurations;
using QuotationSystem.ApplicationCore.Models.Users;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Models.Account;
using System.Linq;
using QuotationSystem.Validators.Account;
using QuotationSystem.Data.Repositories;
using System.Collections.Generic;
using QuotationSystem.ApplicationCore.Constants;

namespace QuotationSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ISessionContext sessionContext;
        private readonly IUserRepository userRepository;
        private readonly IConfigurationContext configuration;

        public AccountController(ISessionContext sessionContext,
            IUserRepository userRepository,
            IConfigurationContext configuration)
        {
            this.sessionContext = sessionContext;
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public IActionResult Login(string returnUrl)
        {
            var user = sessionContext.CurrentUser;

            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };
            return View(model);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.GetUserByUserId(model.UserId);
                sessionContext.CurrentUser = new UserSessionModel
                {
                    Id = user.UserId,
                    RoleIds = user.MUserPermissions
                                 .Where(permission => permission.ActiveStatus == "Y")
                                 .Select(permission => permission.MenuId switch
                                 {
                                     "USS010" => (int)RoleId.UserManagement,
                                     "ITS010" => (int)RoleId.ItemManagement,
                                     "QTS020" => (int)RoleId.QuotationManagement,
                                     _ => throw new System.NotImplementedException()
                                 }).ToList()
                };

                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return RedirectToAction(model.ReturnUrl);
                }
                if (user.ChangePassword == "N")
                {
                    return RedirectToAction("ChangePassword", "Account");
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            model.UserId = sessionContext.CurrentUser.Id;
            var validator = new ChangePasswordValidator(userRepository,sessionContext);
            var result = validator.Validate(model);

            if (result.IsValid)
            {
                userRepository.ChangePassword(model.UserId, model.NewPassword, sessionContext.CurrentUser.Id);
            }

            var errorMessages = string.Join("<br/>", result.Errors.Select(x => x.ErrorMessage));
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            sessionContext.Logout();
            return RedirectToAction(nameof(Login));
        }
    }
}
