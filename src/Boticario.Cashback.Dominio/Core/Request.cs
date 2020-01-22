using Flunt.Notifications;
using MediatR;

namespace Boticario.Cashback.Dominio.Core
{
    public abstract class Request<TResponse> : Notifiable, IRequest<TResponse>
    {

    }
}
