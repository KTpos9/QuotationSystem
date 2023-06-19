using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Zero.Core.Mvc.Jwt
{
    public class JwtContext
    {
        private readonly byte[] _secret;

        public JwtContext(string secret)
        {
            _secret = Encoding.ASCII.GetBytes(secret);
        }

        public string Encode(Claim[] claims)
        {
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload(claims);
            var jwtToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public JwtSecurityToken Decode(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token");
            }
            new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_secret),
                        ValidateAudience = false,
                        ValidateLifetime = false
                    },
                    out var validatedToken);
            return validatedToken as JwtSecurityToken;
        }
    }
}