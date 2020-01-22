using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.RevendedorHandlers.Commands;
using Boticario.Cashback.Dominio.Repositorios;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Handlers.RevendedorHandlers
{
    public class CadastrarRevendedorHandler :
        IRequestHandler<CadastrarRevendedorCommand, Response>
    {
        private readonly IRevendedorRepositorio _repositorio;

        public CadastrarRevendedorHandler(IRevendedorRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Response> Handle(CadastrarRevendedorCommand request, CancellationToken cancellationToken)
        {
            var response = new Response();
            if (request.Invalid)
            {
                response.AddNotifications(request.Notifications);
                return response;
            }

            var revendedor = await _repositorio.ObterPorEmailAsync(request.Email);

            if (revendedor != null)
            {
                response.AddNotification("Jà existe um Revendedor com esse e-mail");
                return response;
            }

            revendedor = new Revendedor(request.Nome, request.Cpf, request.Email, request.Senha);

            await _repositorio.InserirAsync(revendedor);
            response.AddValue(revendedor);
            return response;
        }
    }
}
