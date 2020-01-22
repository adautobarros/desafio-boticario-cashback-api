using Boticario.Cashback.Dominio.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boticario.Cashback.Api.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public ApiBaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<ActionResult> CreatedResponse(Request<Response> command)
        {
            var response = await _mediator.Send(command);
            if (response.Invalid)
                return BadRequest(GetProblemDetails(response));

            return Created($"{ GetType().Name.Replace("Controller", "").ToLower()}/", response.Value);
        }


        private IEnumerable<ProblemDetails> GetProblemDetails(Response response)
        {
            return response.Notifications.Select(item => new ProblemDetails
            {
                Instance = Request.HttpContext.Request.Path,
                Title = item.Message,
                Status = StatusCodes.Status409Conflict,
                Detail = item.Message,
            });
        }

        protected async Task<ActionResult> OkResponse(Request<Response> command)
        {
            var response = await _mediator.Send(command);
            if (response.Invalid)
                return BadRequest(GetProblemDetails(response));

            if (response.Value == null)
                return NoContent();

            return Ok(response.Value);
        }

        protected async Task<ActionResult> NoContentResponse(Request<Response> command)
        {
            var response = await _mediator.Send(command);
            if (response.Invalid)
                return BadRequest(GetProblemDetails(response));



            return NoContent();
        }
    }
}
