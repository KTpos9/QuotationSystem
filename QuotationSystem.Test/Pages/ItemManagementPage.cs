using Microsoft.Playwright;
using System.Threading.Tasks;

namespace QuotationSystem.Test.Pages
{
    public class ItemManagementPage
    {
        private IPage _page;

        public ItemManagementPage(IPage page) => _page = page;

        private ILocator textItemCode => _page.Locator("#textId");
        private ILocator textItemName => _page.Locator("#textName");
        private ILocator btnSearch => _page.Locator("#search-button");

        public async Task Search(string itemCode = "", string itemName = "")
        {
            await textItemCode.FillAsync(itemCode);
            await textItemName.FillAsync(itemName);
            await btnSearch.ClickAsync();
        }
    }
}
