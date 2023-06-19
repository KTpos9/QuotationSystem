using System.Collections.Generic;

namespace Zero.Integration.ScbPayment.Models
{
    public class GenerateQRModel
    {
        public string qrType { get; set; }
        public string ppType { get; set; }
        public string ppId { get; set; }
        public string amount { get; set; }
        public string ref1 { get; set; }
        public string ref2 { get; set; }
        public string ref3 { get; set; }
        public string merchantId { get; set; }
        public string terminalId { get; set; }
        public string invoice { get; set; }
        public string csExtExpiryTime { get; set; } // optional
        public string csNote { get; set; } // optional
        public string csUserDefined { get; set; } // optional
    }

    public class GenerateQRResponseModel
    {
        public StatusResponseModel Status { get; set; }
        public QRResponseModel Data { get; set; }
    }

    public class StatusResponseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class QRResponseModel
    {
        public string QrRawData { get; set; }
        public string QrImage { get; set; }
        public string CsExtExprityTime { get; set; }
        public string ResponseCode { get; set; }
        public string QrCodeType { get; set; }
        public string QrCodeId { get; set; }
        public int Poi { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CsNote { get; set; }
        public string Invoice { get; set; }
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string CsUserDefined { get; set; }
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public List<ChannelModel> Channels { get; set; }
    }

    public class ChannelModel
    {
        public int SeqNo { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
    }
}