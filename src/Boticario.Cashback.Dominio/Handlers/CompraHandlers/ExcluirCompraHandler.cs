using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands;
using Boticario.Cashback.Dominio.Repositorios;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Handlers.CompraHandlers
{
    public class ExcluirCompraHandler : IRequestHandler<ExcluirCompraCommand, Response>
    {
        private readonly ICompraRepositorio _repositorio;

        public ExcluirCompraHandler(ICompraRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Response> Handle(ExcluirCompraCommand request, CancellationToken cancellationToken)
        {
            var response = new Response();
            if (request.Invalid)
            {
                response.AddNotifications(request.Notifications);
                return response;
            }
            var compra = await _repositorio.ObterPorId(request.Id);

            if (compra == null)
            {
                response.AddNotification("Compra não encontrada");
                return response;
            }

            if (compra.Status == StatusCompra.Aprovado)
            {
                response.AddNotification("Compras aprovadas não podem ser excluídas");
                return response;
            }

            await _repositorio.ExcluirAsync(compra);

            return response;
        }
    }
}
