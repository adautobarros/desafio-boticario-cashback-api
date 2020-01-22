using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.RevendedorHandlers.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boticario.Cashback.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiBaseController
    {
        public LoginController(IMediator mediator) : base(mediator) { }

        [HttpPost, AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Revendedor>> PostAsync([FromBody] LoginCommand command)
        {
            return await OkResponse(command);
        }
    }
}