using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Extensions;
using MongoDB.Bson;

namespace Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands
{
    public class ExcluirCompraCommand : Request<Response>
    {
        public ObjectId Id { get; private set; }
        public ExcluirCompraCommand(string id)
        {
            Id = this.ObjectIdCast(id);
        }
    }
}
