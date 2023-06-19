using System;
using System.Collections.Generic;

namespace Zero.Integration.ScbPayment.Models
{
    public class PaymentInquiryModel
    {
        public string EventCode { get; set; }
        public string TransactionDate { get; set; }
        public string BillerId { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string PartnerTransactionId { get; set; } // for promptQR
        public string Amount { get; set; } // optional
        public string QrId { get; set; } // for QRCS
        public string TransactionId { get; set; } // for EasyApp
    }

    public class PaymentInquiryResponseModel
    {
        public StatusResponseModel Status { get; set; }
        public List<InquriyResponseModel> Data { get; set; }
    }

    public class PaymentInquiryQRCSResponseModel
    {
        public StatusResponseModel Status { get; set; }
        public InquiryQRCSResponseModel Data { get; set; }
    }

    public class PaymentInquiryEasyAppResponseModel
    {
        public StatusResponseModel Status { get; set; }
        public InquiryEasyAppResponseModel Data { get; set; }
    }

    public class InquriyResponseModel
    {
        public string EventCode { get; set; }
        public string TransactionType { get; set; }
        public string ReverseFlag { get; set; }
        public string PayeeProxyId { get; set; }
        public string PayeeProxyType { get; set; }
        public string PayeeAccountNumber { get; set; }
        public string PayeeName { get; set; }
        public string PayerProxyId { get; set; }
        public string PayerProxyType { get; set; }
        public string PayerAccountNumber { get; set; }
        public string PayerName { get; set; }
        public string SendingBankCode { get; set; }
        public string ReceivingBankCode { get; set; }
        public string Amount { get; set; }
        public string TransactionId { get; set; }
        public string FastEasySlipNumber { get; set; }
        public string TransactionDateandTime { get; set; }
        public string BillPaymentRef1 { get; set; }
        public string BillPaymentRef2 { get; set; }
        public string BillPaymentRef3 { get; set; }
        public string CurrencyCode { get; set; }
        public string EquivalentAmount { get; set; }
        public string EquivalentCurrencyCode { get; set; }
        public string ExchangeRate { get; set; }
        public string ChannelCode { get; set; }
        public string PartnerTransactionId { get; set; }
        public string TepaCode { get; set; }
    }

    public class InquiryQRCSResponseModel
    {
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string TransactionDateAndTime { get; set; }
        public string MerchantPAN { get; set; }
        public string ConsumerPAN { get; set; }
        public string CurrencyCode { get; set; }
        public string MerchantId { get; set; }
        public string TerminalId { get; set; }
        public string QrId { get; set; }
        public string TraceNo { get; set; }
        public string AuthorizeCode { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionType { get; set; }
        public string ChannelCode { get; set; }
        public string Invoice { get; set; }
        public string Note { get; set; }
    }

    public class InquiryEasyAppResponseModel
    {
        public string TransactionId { get; set; }
        public string UserRefId { get; set; }
        public string TransactionType { get; set; }
        public List<string> TransactionSubType { get; set; }
        public string TransactionMethod { get; set; }
        public decimal PaidAmount { get; set; }
        public string AccountFrom { get; set; }
        public BillPaymentModel BillPayment { get; set; }
        public CreditCardModel CreditCardFullAmount { get; set; }
        public InstallPaymentPlanModel InstallPaymentPlan { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public int SessionValidityPeriod { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
        public int StatusCode { get; set; }
        public MerchantMetaDataModel MerchantMetaData { get; set; }
        public string Status => GetStatus(StatusCode);

        private string GetStatus(int code)
        {
            switch (code)
            {
                case 0: return "PENDING";
                case 1: return "PAID";
                case 2: return "CANCELLED";
                case 3: return "INVALID";
                case 4: return "PARTIAL";
                case 5: return "EXPIRED";
                default: return "";
            }
        }
    }
}