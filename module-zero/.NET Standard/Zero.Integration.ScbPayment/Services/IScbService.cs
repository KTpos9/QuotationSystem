using System;
using Zero.Integration.ScbPayment.Models;

namespace Zero.Integration.ScbPayment.Services
{
    public interface IScbService
    {
        GenerateQRResponseModel GenerateQR(Credential credential, Guid requestUId, GenerateQRModel model);

        PaymentInquiryResponseModel GetInquiryStatus(Credential credential, Guid requestUId, PaymentInquiryModel model);

        PaymentInquiryQRCSResponseModel GetInquiryQRCSStatus(Credential credential, Guid requestUId, PaymentInquiryModel model);

        PaymentInquiryEasyAppResponseModel GetInquiryEasyAppStatus(Credential credential, Guid requestUId, PaymentInquiryModel model);

        DeepLinkResponseModel GetDeepLinkUrl(Credential credential, Guid requestUId, DeepLinkModel model);
    }
}