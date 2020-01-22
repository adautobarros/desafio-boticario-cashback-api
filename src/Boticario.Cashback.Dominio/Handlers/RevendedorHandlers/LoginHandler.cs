using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.RevendedorHandlers.Commands;
using Boticario.Cashback.Dominio.Repositorios;
using Boticario.Cashback.Dominio.Servicos;
using Boticario.Cashback.Dominio.ValueObjects;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Handlers.RevendedorHandlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, Response>
    {
        private readonly IJwtService _jwtService;
        private readonly IRevendedorRepositorio _revendedorRepositorio;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginHandler(
            IJwtService jwtService,
            IRevendedorRepositorio revendedorRepositorio,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtService = jwtService;
            _revendedorRepositorio = revendedorRepositorio;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Response> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var response = new Response();

            Revendedor revendedor = null;

            if (command.GrantType.Equals("password"))
            {
                revendedor = await HandleRevendedorAuthentication(response, command);
            }
            else if (command.GrantType.Equals("refresh_token"))
            {
                revendedor = await HandleRefreshToken(response, command);
            }

            if (response.Invalid || revendedor == null)
            {
                return response;
            }

            await HandleJwt(response, revendedor);

            return response;
        }

        private async Task HandleJwt(Response response, Revendedor revendedor)
        {
            var jwt = _jwtService.CreateJsonWebToken(revendedor);
            await _refreshTokenRepository.Salvar(jwt.RefreshToken);

            response.AddValue(new
            {
                access_token = jwt.AccessToken,
                refresh_token = jwt.RefreshToken.Token,
                token_type = jwt.TokenType,
                expires_in = jwt.ExpiresIn,
                id = revendedor.Id,
                nome = revendedor.Nome,
            });
        }

        private async Task<Revendedor> HandleRevendedorAuthentication(Response response, LoginCommand command)
        {
            var revendedor = await _revendedorRepositorio.ObterPorEmailAsync(command.Email);
            var senha = new Senha(command.Senha);

            if (revendedor == null || !revendedor.Senha.Equals(senha))
            {
                response.AddNotification("E-mail ou senha inválidos");
            }

            return revendedor;
        }

        private async Task<Revendedor> HandleRefreshToken(Response response, LoginCommand command)
        {
            var token = await _refreshTokenRepository.Obter(command.RefreshToken);

            if (token == null)
            {
                response.AddNotification("Login inválido");
            }
            else if (token.DataExpiracao < DateTime.UtcNow)
            {
                response.AddNotification("Login expirado");
            }

            if (response.Invalid)
            {
                return null;
            }

            return await _revendedorRepositorio.ObterPorEmailAsync(token.Email);
        }

    }
}
