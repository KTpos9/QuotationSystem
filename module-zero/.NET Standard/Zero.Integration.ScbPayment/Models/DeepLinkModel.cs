using System.Collections.Generic;

namespace Zero.Integration.ScbPayment.Models
{
    public class DeepLinkModel
    {
        public string transactionType { get; set; }
        public List<string> transactionSubType { get; set; }
        public int sessionValidityPeriod { get; set; } //optional
        public int sessionValidUntil { get; set; } //optional
        public BillPaymentModel billPayment { get; set; } // for BP
        public CreditCardModel creditCardFullAmount { get; set; } // for CCFA
        public InstallPaymentPlanModel installPaymentPlan { get; set; } // for CCIPP
        public MerchantMetaDataModel merchantMetaData { get; set; } // optional
    }

    public class BillPaymentModel
    {
        public decimal paymentAmount { get; set; }
        public string accountTo { get; set; }
        public string accountFrom { get; set; } //optional
        public string ref1 { get; set; }
        public string ref2 { get; set; } //optional
        public string ref3 { get; set; } //optional
    }

    public class CreditCardModel
    {
        public string merchantId { get; set; }
        public string terminalId { get; set; } //optional
        public string orderReference { get; set; }
        public decimal paymentAmount { get; set; }
    }

    public class InstallPaymentPlanModel
    {
        public string merchantId { get; set; }
        public string terminalId { get; set; } //optional
        public string orderReference { get; set; }
        public decimal paymentAmount { get; set; }
        public string tenor { get; set; } // where ipp Type is not null
        public string ippType { get; set; }
        public string prodCode { get; set; } // where ipp Type is "3"
    }

    public class MerchantMetaDataModel
    {
        public string callbackUrl { get; set; }
        public List<MerchantInfoModel> merchantInfo { get; set; }
    }

    public class MerchantInfoModel
    {
        public string name { get; set; }
    }

    public class DeepLinkResponseModel
    {
        public StatusResponseModel Status { get; set; }
        public DeepLinkUrlModel Data { get; set; }
    }

    public class DeepLinkUrlModel
    {
        public string UserRefId { get; set; }
        public string TransactionId { get; set; }
        public string DeepLinkUrl { get; set; }
    }
}