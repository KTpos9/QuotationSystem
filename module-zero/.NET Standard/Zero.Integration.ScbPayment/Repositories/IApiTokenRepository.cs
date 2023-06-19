using Zero.Integration.ScbPayment.Models;

namespace Zero.Integration.ScbPayment.Repositories
{
    public interface IApiTokenRepository
    {
        TokenModel GetAccessToken(string clientId);

        void Update(TokenModel token);
    }
}