using Microsoft.Playwright;
using System.Threading.Tasks;

namespace QuotationSystem.Test.Pages
{
    public class LoginPage
    {
        private IPage _page;

        private ILocator textUsername => _page.Locator("#UserId");
        private ILocator textPassword => _page.Locator("#Password");
        private ILocator btnLogin => _page.Locator("text=Login");
        public LoginPage(IPage page) => _page = page;

        public async Task Login(string username, string password)
        {
            await textUsername.FillAsync(username);
            await textPassword.FillAsync(password);
            await btnLogin.ClickAsync();
        }
    }
}
