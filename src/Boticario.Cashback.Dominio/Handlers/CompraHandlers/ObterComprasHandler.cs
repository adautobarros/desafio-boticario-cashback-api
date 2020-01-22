using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Queries;
using Boticario.Cashback.Dominio.Repositorios;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Handlers.CompraHandlers
{
    public class ObterComprasHandler : IRequestHandler<ObterComprasQuery, Response>
    {
        private readonly ICompraRepositorio _repositorio;

        public ObterComprasHandler(ICompraRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Response> Handle(ObterComprasQuery request, CancellationToken cancellationToken)
        {
            var response = new Response();
            var compras = await _repositorio.ObterItensAsync();
            response.AddValue(compras);

            return response;
        }
    }
}
