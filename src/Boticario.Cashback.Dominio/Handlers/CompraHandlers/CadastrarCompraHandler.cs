using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands;
using Boticario.Cashback.Dominio.Repositorios;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Handlers.CompraHandlers
{
    public class CadastrarCompraHandler : IRequestHandler<CadastrarCompraCommand, Response>
    {
        private readonly ICompraRepositorio _repositorio;

        public CadastrarCompraHandler(ICompraRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Response> Handle(CadastrarCompraCommand request, CancellationToken cancellationToken)
        {
            var response = new Response();
            if (request.Invalid)
            {
                response.AddNotifications(request.Notifications);
                return response;
            }

            var compra = await _repositorio.ObterPorCodigo(request.Codigo);

            if (compra != null)
            {
                response.AddNotification("Jà existe uma Compra com esse código");
                return response;
            }

            compra = new Compra(request.Codigo, request.Valor, request.Data, request.Cpf);
            await _repositorio.InserirAsync(compra);
            response.AddValue(compra);
            return response;
        }
    }
}
