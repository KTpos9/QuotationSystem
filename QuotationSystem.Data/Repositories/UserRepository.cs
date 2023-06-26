using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;
using Zero.Core.Mvc.Models.Select2;
using Zero.Extension;
using Zero.Security;

namespace QuotationSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const int PasswordSaltLength = 32;
        private readonly DbContextOptionBuilder option;

        public UserRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }

        public List<Select2Model> GetUsernameItems()
        {
            using (var db = new QuotationContext(option))
            {
                return db.MUsers.Select(x => new Select2Model
                {
                    Id = x.UserName,
                    Text = x.UserName
                })
                .ToList();
            }
        }

        public List<MUser> GetUsers()
        {
            using (var db = new QuotationContext(option))
            {
                return db.MUsers.Include(u => u.MUserPermissions).ToList();
            }
        }

        public MUser GetUserByUserId(string userId)
        {
            using (var db = new QuotationContext(option))
            {
                //return db.MUsers.Find(userId);
                return db.MUsers.Include(u => u.MUserPermissions).FirstOrDefault(x => x.UserId == userId);
            }
        }

        public void ChangePassword(string userId, string password, string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                //var user = db.MUsers.FirstOrDefault(x => x.UserId == userId);
                var user = db.MUsers.Find(userId);

                var hashedPassword = PasswordEncryption.Hash(password, "");
                user.Password = hashedPassword;
                user.ChangePassword = "Y";
                db.SaveChanges();
            }
        }

        public void AddUser(MUser user, string[] permissions,  string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                db.CurrentUser = currentUser;

                user.MUserPermissions = db.MMenus.Select(m => new MUserPermission
                {
                    MenuId = m.MenuId
                }).ToList();

                user.ActiveStatus = user.ActiveStatus switch
                {
                    "false" => "N",
                    _ => "Y"
                };

                foreach (var permission in permissions)
                {
                    user.MUserPermissions.Where(x => x.MenuId == permission).FirstOrDefault().ActiveStatus = "Y";
                }
                db.Add(user);
                db.SaveChanges();
            }
        }
        public void EditUser(MUser user, string[] permissions, string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                var userToUpdate = db.MUsers.Include(u => u.MUserPermissions).FirstOrDefault(x => x.UserId == user.UserId);
                userToUpdate.UserName = user.UserName;
                userToUpdate.DepartmentId = user.DepartmentId;

                userToUpdate.ActiveStatus = user.ActiveStatus switch
                {
                    "false" => "N",
                    _ => "Y"
                };

                foreach (var userPermission in userToUpdate.MUserPermissions)
                {
                    userPermission.ActiveStatus = permissions.Any(p => p == userPermission.MenuId) switch
                    {
                        true => "Y",
                        _ => "N"
                    };
                }

                db.CurrentUser = currentUser;
                db.SaveChanges();
            }
        }

        public void ResetPassword(string userId, string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                var user = db.MUsers.Find(userId);
                var defaultPassword = db.CConfigs.FirstOrDefault(c => c.ConfCode == "C001");
                user.Password = PasswordEncryption.Hash(defaultPassword.ConfValue, "");
                user.ChangePassword = "N";
                db.CurrentUser = currentUser;
                db.SaveChanges();
            }
        }

        public void DeleteUser(string userId, string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                var user = db.MUsers.Find(userId);
                db.Remove(user);
                db.SaveChanges();
            }
        }

        public DataTableResultModel<MUser> GetUserList(DataTableOptionModel dtOption, string username = "", string userId = "", string department = "")
        {
            using (var db = new QuotationContext(option))
            {
                return db.MUsers.Include(u => u.MUserPermissions)
                    .WhereIf(string.IsNullOrEmpty(userId) == false, x => x.UserId.Contains(userId))
                    .WhereIf(string.IsNullOrEmpty(username) == false, x => x.UserName.Contains(username))
                    .WhereIf(string.IsNullOrEmpty(department) == false, x => x.DepartmentId.Contains(department))
                    .Select(u => new MUser { 
                        UserId = u.UserId,
                        UserName = u.UserName,
                        DepartmentId = u.DepartmentId,
                        ActiveStatus = u.ActiveStatus,
                        ChangePassword = u.ChangePassword,
                        MUserPermissions = u.MUserPermissions
                    })
                    .ToDataTableResult(dtOption);
            }
        }
    }
}