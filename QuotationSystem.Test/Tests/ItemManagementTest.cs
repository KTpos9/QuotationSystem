using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using QuotationSystem.Test.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.Test.Tests
{
    public class ItemManagementTest : PageTest
    {
        private readonly string webUrl = "http://localhost:5000";
        [SetUp]
        public async Task SetupAsync()
        {
            await Page.GotoAsync(webUrl);
            var loginPage = new LoginPage(Page);
            await loginPage.Login("EMP001", "234567");
        }
        [Test]
        public async Task SearchItemCode()
        {
            await Page.GotoAsync($"{webUrl}/User/UserList");
            var userManagement = new UserManagementPage(Page);
            await userManagement.Search(userID: "EMP001");
            await Expect(Page.Locator("#user-table>tbody>tr:nth-child(1)>td:nth-child(2)")).ToHaveTextAsync("EMP001");
        }
        public async Task SearchItemName()
        {
            await Page.GotoAsync($"{webUrl}/Ttem/ItemList");
            var userManagement = new ItemManagementPage(Page);
            await userManagement.Search(itemCode: "");
            await Expect(Page.Locator("#user-table>tbody>tr:nth-child(1)>td:nth-child(2)")).ToHaveTextAsync("EMP001");
        }
        [TearDown]
        public virtual async Task ClearUp()
        {
            await Page.CloseAsync();
        }
    }
}
