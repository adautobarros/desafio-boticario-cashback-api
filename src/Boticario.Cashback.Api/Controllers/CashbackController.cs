using Boticario.Cashback.Dominio.Extensions;
using Boticario.Cashback.Dominio.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boticario.Cashback.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CashbackController : ControllerBase
    {
        private readonly IBoticarioApiService _boticarioApiService;

        public CashbackController(IBoticarioApiService boticarioApiService)
        {
            _boticarioApiService = boticarioApiService;
        }

        [HttpGet("{cpf}/acumulado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<decimal>> GetAsync(string cpf)
        {
            cpf = cpf?.Trim().SomenteNumeros();
            var credit = await _boticarioApiService.Cashback(cpf);
            if (credit != null)
            {
                return Ok(credit);
            }
            return NoContent();
        }
    }
}
