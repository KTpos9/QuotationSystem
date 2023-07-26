using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Zero.Core.Mvc.Authorizations.Contexts;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;
using QuotationSystem.ApplicationCore.Models.Users;

namespace QuotationSystem.Data.Sessions
{
    public class SessionContext : ISessionContext, IRolePolicyContext
    {
        private readonly ISession session;

        public SessionContext(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor?.HttpContext?.Session;
        }

        public UserSessionModel CurrentUser
        {
            get => session?.GetObjectFromJson<UserSessionModel>(nameof(CurrentUser));
            set => session?.SetObjectAsJson(nameof(CurrentUser), value);
        }

        public bool IsLoggedIn => CurrentUser != null;

        public List<int> Roles => CurrentUser?.RoleIds;

        public void Logout()
        {
            session.Clear();
        }

        public T GetCriteria<T>(string key, Func<T> createDefaultObject) where T : class, new()
        {
            return session.GetCriteria(key, createDefaultObject);
        }

        public void SaveCriteria<T>(string key, T criteria) where T : class, new()
        {
            session.SaveCriteria(key, criteria);
        }

        public void SaveSearchingOption<T>(string key, T criteria, DataTableOptionModel option) where T : class, new()
        {
            session.SaveCriteria(key, criteria);
            session.SaveDataTableOption(key, option);
        }

        public DataTableOptionModel GetDataTableOption(string key)
        {
            return session.GetDataTableOption(key);
        }
    }
}