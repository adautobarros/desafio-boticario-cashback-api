using Boticario.Cashback.Dominio.Core;
using Flunt.Notifications;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boticario.Cashback.Api.Middlewares
{
    public class RequestsValidationMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : Request<Response>
        where TResponse : Response
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            return request.Invalid
                ? Notifications(request.Notifications)
                : next();
        }

        private static Task<TResponse> Notifications(IEnumerable<Notification> notifications)
        {
            var response = new Response();
            response.AddNotifications(notifications);

            return Task.FromResult(response as TResponse);
        }
    }
}
