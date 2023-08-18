using Microsoft.Playwright;
using System.Threading.Tasks;

namespace QuotationSystem.Test.Pages
{
    public class QuotationManagementPage
    {
        private IPage _page; 
        public QuotationManagementPage(IPage page) => _page = page;

        public async Task Search(string searchTerm)
        {

        }
    }
}
