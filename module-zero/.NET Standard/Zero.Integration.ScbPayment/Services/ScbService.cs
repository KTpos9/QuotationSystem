using Newtonsoft.Json;
using RestSharp;
using System;
using Zero.Integration.ScbPayment.Models;
using Zero.Integration.ScbPayment.Repositories;

namespace Zero.Integration.ScbPayment.Services
{
    public class ScbService : IScbService
    {
        private readonly ScbConfiguration configuration;
        private readonly IApiTokenRepository apiTokenRepository;

        public ScbService(ScbConfiguration configuration,
            IApiTokenRepository apiTokenRepository)
        {
            this.configuration = configuration;
            this.apiTokenRepository = apiTokenRepository;
        }

        public GenerateQRResponseModel GenerateQR(Credential credential, Guid requestUId, GenerateQRModel model)
        {
            var accessToken = GetAccessToken(credential);
            return Execute<GenerateQRResponseModel>("/v1/payment/qrcode/create", Method.POST, credential.ClientId, accessToken, requestUId, (req) =>
            {
                req.AddJsonBody(model);
            });
        }

        public PaymentInquiryResponseModel GetInquiryStatus(Credential credential, Guid requestUId, PaymentInquiryModel model)
        {
            var accessToken = GetAccessToken(credential);
            return Execute<PaymentInquiryResponseModel>("/v1/payment/billpayment/inquiry", Method.GET, credential.ClientId, accessToken, requestUId, (req) =>
            {
                req.AddParameter("eventCode", model.EventCode);
                req.AddParameter("billerId", model.BillerId);
                req.AddParameter("reference1", model.Reference1);
                req.AddParameter("reference2", model.Reference2);
                req.AddParameter("transactionDate", model.TransactionDate);
            });
        }

        public PaymentInquiryQRCSResponseModel GetInquiryQRCSStatus(Credential credential, Guid requestUId, PaymentInquiryModel model)
        {
            var accessToken = GetAccessToken(credential);
            return Execute<PaymentInquiryQRCSResponseModel>($"/v1/payment/qrcode/creditcard/{model.QrId}", Method.GET, credential.ClientId, accessToken, requestUId, (req) => { });
        }

        public PaymentInquiryEasyAppResponseModel GetInquiryEasyAppStatus(Credential credential, Guid requestUId, PaymentInquiryModel model)
        {
            var accessToken = GetAccessToken(credential);
            return Execute<PaymentInquiryEasyAppResponseModel>($"/v2/transactions/{model.TransactionId}", Method.GET, credential.ClientId, accessToken, requestUId, (req) => { });
        }

        public DeepLinkResponseModel GetDeepLinkUrl(Credential credential, Guid requestUId, DeepLinkModel model)
        {
            var accessToken = GetAccessToken(credential);
            return Execute<DeepLinkResponseModel>("/v3/deeplink/transactions", Method.POST, credential.ClientId, accessToken, requestUId, (req) =>
            {
                req.AddHeader("channel", "scbeasy");
                req.AddJsonBody(model);
            });
        }

        private string GetAccessToken(Credential credential)
        {
            var dbAccessToken = apiTokenRepository.GetAccessToken(credential.ClientId);
            if (dbAccessToken?.ExpiresAt == null || dbAccessToken.ExpiresAt?.AddMinutes(-1) <= DateTime.Now)
            {
                // generate new token
                var client = new RestClient(configuration.BaseUrl);
                var request = new RestRequest("/v1/oauth/token", Method.POST);
                request.AddHeader("resourceOwnerId", credential.ClientId);
                request.AddHeader("requestUId", Guid.NewGuid().ToString());
                request.AddJsonBody(new ApiTokenModel(credential.ClientId, credential.ClientSecret));
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var apiToken = JsonConvert.DeserializeObject<ApiTokenResponseModel>(response.Content);
                    apiTokenRepository.Update(new TokenModel(credential.ClientId,
                        apiToken.Data.AccessToken,
                        apiToken.Data.TokenType,
                        apiToken.Data.ExpiresIn,
                        DateTime.Now.AddSeconds(apiToken.Data.ExpiresIn))
                        );
                    return apiToken.Data.AccessToken;
                }
            }
            return dbAccessToken.AccessToken;
        }

        private T Execute<T>(string url, Method method, string clientId,
            string accessToken, Guid requestUId, Action<RestRequest> addingParameter) where T : new()
        {
            var client = new RestClient(configuration.BaseUrl);
            var request = new RestRequest(url, method);
            request.AddHeader("authorization", $"Bearer {accessToken}");
            request.AddHeader("resourceOwnerId", clientId);
            request.AddHeader("requestUId", requestUId.ToString());

            if (addingParameter != null)
            {
                addingParameter(request);
            }

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            return default;
        }
    }
}