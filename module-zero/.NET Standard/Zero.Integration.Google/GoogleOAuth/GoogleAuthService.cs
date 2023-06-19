using Newtonsoft.Json;
using RestSharp;

namespace Zero.Integration.Google.GoogleOAuth
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public string GetAuthUrl(string authUrl, GoogleCredential credential)
        {
            return string.Format(authUrl, credential.RedirectUrl, credential.ClientId);
        }

        public GoogleAccessToken GetToken(string tokenUrl, string code, GoogleCredential credential)
        {
            var client = new RestClient(tokenUrl);
            var request = new RestRequest(Method.POST);
            request.AddParameter("client_id", credential.ClientId);
            request.AddParameter("client_secret", credential.ClientSecret);
            request.AddParameter("redirect_uri", credential.RedirectUrl);
            request.AddParameter("code", code);
            request.AddParameter("grant_type", "authorization_code");

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var json = response.Content;
                GoogleAccessToken acessToken = JsonConvert.DeserializeObject<GoogleAccessToken>(json);
                return acessToken;
            }

            return null;
        }

        /// <summary>
        /// Get user info after signed-in by returned access_token
        /// </summary>
        /// <param name="userInfoUrl"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public GoogleUserInfo GetUserInfo(string userInfoUrl, string accessToken)
        {
            var url = string.Format(userInfoUrl, accessToken);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            var response = client.Get(request);
            if (response.IsSuccessful)
            {
                string json = response.Content;
                GoogleUserInfo info = JsonConvert.DeserializeObject<GoogleUserInfo>(json);
                if (info != null)
                {
                    return info;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Token and UserInfo in 1 step
        /// </summary>
        /// <param name="code"></param>
        /// <param name="url"></param>
        /// <param name="credential"></param>
        /// <returns></returns>
        public GoogleUserInfo GetUserInfo(string code, GoogleApiUrl url, GoogleCredential credential)
        {
            var accessToken = GetToken(url.Token, code, credential);
            if (accessToken != null)
            {
                var userInfo = GetUserInfo(url.UserInfo, accessToken.AccessToken);
                if (userInfo != null)
                {
                    return userInfo;
                }
            }

            return null;
        }
    }
}