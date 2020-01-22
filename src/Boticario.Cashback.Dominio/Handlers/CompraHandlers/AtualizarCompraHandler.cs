using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands;
using Boticario.Cashback.Dominio.Repositorios;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Handlers.CompraHandlers
{
    public class AtualizarCompraHandler : IRequestHandler<AtualizarCompraCommand, Response>
    {
        private readonly ICompraRepositorio _repositorio;

        public AtualizarCompraHandler(ICompraRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Response> Handle(AtualizarCompraCommand request, CancellationToken cancellationToken)
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
                response.AddNotification("Compras aprovadas não podem ser editadas");
                return response;
            }

            var compraCadastrada = await _repositorio.ObterPorCodigo(request.Codigo);

            if (compraCadastrada != null && compraCadastrada.Codigo != compra.Codigo)
            {
                response.AddNotification("Jà existe uma Compra com esse código");
                return response;
            }

            compra.Atualizar(request.Codigo, request.Valor, request.Data, request.Cpf);
            await _repositorio.AtualizarAsync(compra);
            response.AddValue(compra);

            return response;
        }
    }
}
