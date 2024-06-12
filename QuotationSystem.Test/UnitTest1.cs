using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using QuotationSystem.Test.Pages;
using System.Threading.Tasks;

namespace QuotationSystem.Test
{
    [Parallelizable(ParallelScope.Self)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [TestFixture]
    public class UnitTest1 : PageTest
    {
        private readonly string webUrl = "http://localhost:5000/";
        [SetUp]
        public async Task SetupAsync()
        {
            await Page.GotoAsync(webUrl);
            var loginPage = new LoginPage(Page);
            await loginPage.Login("EMP001","234567");
        }

        [Test]
        public async Task HomePage()
        {
            await Expect(Page).ToHaveURLAsync(webUrl);
            //await Expect(Page.Locator("")).ToContainTextAsync("EMP001");
            //await Expect(Page.Locator("")).ToContainTextAsync("EMP001");
        }
        [Test]
        public async Task UserManagement_SearchForId()
        {
            await Page.GotoAsync($"{webUrl}User/UserList");
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