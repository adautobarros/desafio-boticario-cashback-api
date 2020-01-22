using Boticario.Cashback.Dominio.Entidades;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Repositorios
{
    public interface ICompraRepositorio
    {
        Task<Compra> ObterPorId(ObjectId id);
        Task<Compra> ObterPorCodigo(string codigo);
        Task<ICollection<Compra>> ObterItensAsync();
        Task InserirAsync(Compra compra);
        Task AtualizarAsync(Compra compra);
        Task ExcluirAsync(Compra compra);
    }
}
