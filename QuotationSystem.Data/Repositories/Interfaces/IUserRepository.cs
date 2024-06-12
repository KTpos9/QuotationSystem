using QuotationSystem.Data.Models;
using System.Collections.Generic;
using Zero.Core.Mvc.Models.DataTables;
using Zero.Core.Mvc.Models.Select2;

namespace QuotationSystem.Data.Repositories
{
    public interface IUserRepository
    {
        MUser GetUserByUserId(string username);
        void ChangePassword(string userId, string password, string currentUser);
        List<Select2Model> GetUsernameItems();
        void AddUser(MUser user, string[] permissions);
        void ResetPassword(string userId, string currentUser);
        void EditUser(MUser user, string[] permissions);
        void DeleteUser(string userId);
        DataTableResultModel<MUser> GetUserList(DataTableOptionModel dtOption, string username = "", string userId = "", string department = "");
    }
}