using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using QuotationSystem.Test.Pages;
using System.Threading.Tasks;

namespace QuotationSystem.Test.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [TestFixture]
    public class UserManagementTest : PageTest
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
        public async Task SearchForId()
        {
            await Page.GotoAsync($"{webUrl}/User/UserList");
            var userManagement = new UserManagementPage(Page);
            await userManagement.Search(userID: "EMP001");
            await Expect(Page.Locator("#user-table>tbody>tr:nth-child(1)>td:nth-child(2)")).ToHaveTextAsync("EMP001");
        }
        [TearDown]
        public virtual async Task ClearUp()
        {
            await Page.CloseAsync();
        }
    }
}
