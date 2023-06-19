namespace Zero.Integration.Google.GoogleOAuth
{
    public interface IGoogleAuthService
    {
        string GetAuthUrl(string authUrl, GoogleCredential credential);

        GoogleAccessToken GetToken(string tokenUrl, string code, GoogleCredential credential);

        GoogleUserInfo GetUserInfo(string userInfoUrl, string accessToken);

        GoogleUserInfo GetUserInfo(string code, GoogleApiUrl url, GoogleCredential credential);
    }
}