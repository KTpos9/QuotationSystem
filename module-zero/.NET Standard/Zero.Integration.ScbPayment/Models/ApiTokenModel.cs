namespace Zero.Integration.ScbPayment.Models
{
    public record ApiTokenModel(string applicationKey, string applicationSecret);

    public record ApiTokenResponseModel(StatusResponseModel Status, TokenResponseModel Data);

    public record TokenResponseModel(string ClientId, string AccessToken, string TokenType, int ExpiresIn);
}