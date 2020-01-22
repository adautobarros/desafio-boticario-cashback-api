using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boticario.Cashback.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ComprasController : ApiBaseController
    {
        public ComprasController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Compra>>> GetAsync([FromQuery] ObterComprasQuery query)
        {
            return await OkResponse(query);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Compra>> PostAsync([FromBody] CadastrarCompraCommand command)
        {
            return await CreatedResponse(command);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Compra>> PutAsync(string id, [FromBody] AtualizarCompraCommand command)
        {
            command.AdicionarId(id);
            return await OkResponse(command);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var command = new ExcluirCompraCommand(id);
            return await NoContentResponse(command);
        }
    }
}
