using Microsoft.Playwright;
using System.Threading.Tasks;

namespace QuotationSystem.Test.Pages
{
    public class UserManagementPage
    {
        private IPage _page;

        public UserManagementPage(IPage page) => _page = page;

        private ILocator btnSearch => _page.Locator("#search-button");
        private ILocator textUserId => _page.Locator("#textId");
        private ILocator textName => _page.Locator("#textName");
        private ILocator textDepartment => _page.Locator("#textDepartment");

        public async Task Search(string userID = "", string name = "", string department = "")
        {
            await textUserId.FillAsync(userID);
            await textName.FillAsync(name);
            await textDepartment.FillAsync(department);
            await btnSearch.ClickAsync();
        }
    }
}
