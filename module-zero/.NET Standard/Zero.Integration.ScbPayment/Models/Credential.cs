using System;

namespace Zero.Integration.ScbPayment.Models
{
    public record Credential(string ClientId, string ClientSecret);

    public record TokenModel(string ClientId, string AccessToken, string TokenType, int ExpiresIn, DateTime? ExpiresAt);
}