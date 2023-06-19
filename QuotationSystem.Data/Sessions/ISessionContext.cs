using QuotationSystem.ApplicationCore.Models.Users;
using System;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Data.Sessions
{
    public interface ISessionContext
    {
        UserSessionModel CurrentUser { get; set; }
        bool IsLoggedIn { get; }

        T GetCriteria<T>(string key, Func<T> createDefaultObject) where T : class, new();
        DataTableOptionModel GetDataTableOption(string key);
        void Logout();
        void SaveCriteria<T>(string key, T criteria) where T : class, new();
        void SaveSearchingOption<T>(string key, T criteria, DataTableOptionModel option) where T : class, new();
    }
}