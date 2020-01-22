using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Model;
using Boticario.Cashback.Dominio.Servicos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Boticario.Cashback.Infra.Servicos
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(JwtSettings settings)
        {
            _settings = settings;
        }

        public JsonWebToken CreateJsonWebToken(Revendedor usuario)
        {
            var identity = GetClaimsIdentity(usuario);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identity,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                IssuedAt = _settings.IssuedAt,
                NotBefore = _settings.NotBefore,
                Expires = _settings.AccessTokenExpiration,
                SigningCredentials = _settings.SigningCredentials
            });

            var accessToken = handler.WriteToken(securityToken);

            return new JsonWebToken
            {
                AccessToken = accessToken,
                RefreshToken = CreateRefreshToken(usuario.Email),
                ExpiresIn = TimeSpan.FromMinutes(_settings.ValidForMinutes).Ticks
            };
        }

        private RefreshToken CreateRefreshToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                Email = email,
                DataExpiracao = _settings.RefreshTokenExpiration
            };

            string token;
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.Token = token.Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);

            return refreshToken;
        }

        private static ClaimsIdentity GetClaimsIdentity(Revendedor usuario)
        {
            var identity = new ClaimsIdentity
            (
                new GenericIdentity(usuario.Email),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome),
                }
            );

            return identity;
        }
    }
}
