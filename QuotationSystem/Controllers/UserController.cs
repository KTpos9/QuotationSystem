using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Models.User;
using System.Linq;
using Zero.Security;
using Zero.Core.Mvc.Models.DataTables;
using Zero.Core.Mvc.Extensions;
using QuotationSystem.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace QuotationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IConfigRepository configRepository;
        private readonly IDepartmentRepository departmentRepository;
        private static List<SelectListItem> departmentList;
        private static string CurrentUser;

        public UserController(IUserRepository userRepository, ISessionContext sessionContext, IDepartmentRepository departmentRepository, IConfigRepository configRepository)
        {
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            CurrentUser = sessionContext.CurrentUser.Id;
            this.configRepository = configRepository;
        }
        public IActionResult UserList()
        {
            ViewBag.DepartmentList = departmentRepository.GetAllDepartmentIds();
            departmentList = departmentRepository.GetAllDepartmentIds();
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(UserModalViewModel userModel, string[] permissions)
        {
            
            if (ModelState.IsValid)
            {
                string defaultPassword = PasswordEncryption.Hash(configRepository.GetDefaultPassowrd(), "");
                MUser userToAdd = new MUser
                {
                    UserId = userModel.UserId,
                    UserName = userModel.Username,
                    Password = defaultPassword,
                    DepartmentId = userModel.Department,
                    ActiveStatus = userModel.ActiveStatus,
                    CreateBy = CurrentUser,
                    UpdateBy = CurrentUser
                };
                userRepository.AddUser(userToAdd, permissions);
                return RedirectToAction("UserList");
            }
            return PartialView("_AddUserPartial");
        }
        [HttpPost]
        public JsonResult Search(string userId, string name, string department , DataTableOptionModel option)
        {
            var result = userRepository.GetUserList(option, username: name, userId: userId, department: department);
            var response = result.ToJsonResult(option);
            return response;
        }
        public PartialViewResult GetResetPasswordModal(string userId)
        {
            var model = new UserViewModel
            {
                UserId = userId,
            };
            return PartialView("_ResetPasswordPartial", model);
        }
        public PartialViewResult GetEditUserModal(string userId)
        {
            var user = userRepository.GetUserByUserId(userId);

            var model = new UserViewModel
            {
                UserId = user.UserId,
                User = user,
                DepartmentIds = departmentList
            };
            return PartialView("_EditUserPartial", model);
        }
        public PartialViewResult GetDeleteUserModal(string userId)
        {
            var model = new UserViewModel
            {
                UserId = userId
            };
            return PartialView("_DeleteUserPartial", model);
        }
        [HttpPost]
        public IActionResult EditUser(UserViewModel userModel, string[] permissions)
        {
            userModel.User.UpdateBy = CurrentUser;
            userRepository.EditUser(userModel.User, permissions);
            return RedirectToAction("UserList");
        }
        [HttpPost]
        public IActionResult ResetPassword(string userId)
        {
            userRepository.ResetPassword(userId, CurrentUser);
            return RedirectToAction("UserList","User");
        }
        [HttpDelete]
        public IActionResult DeleteUser(string userId)
        {
            userRepository.DeleteUser(userId);
            return RedirectToAction("UserList", "User");
        }
    }
}
