using Microsoft.Playwright;
using System.Threading.Tasks;

namespace QuotationSystem.Test.Pages
{
    public class ItemManagementPage
    {
        private IPage _page;

        public ItemManagementPage(IPage page) => _page = page;
    }
}
