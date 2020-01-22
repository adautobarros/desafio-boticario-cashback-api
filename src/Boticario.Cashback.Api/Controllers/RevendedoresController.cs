using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.RevendedorHandlers.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boticario.Cashback.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class RevendedoresController : ApiBaseController
    {
        public RevendedoresController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Revendedor>> PostAsync([FromBody] CadastrarRevendedorCommand command)
        {
            return await CreatedResponse(command);
        }
    }
}
