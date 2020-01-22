using Boticario.Cashback.Dominio.Entidades;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Repositorios
{
    public interface IRevendedorRepositorio
    {
        Task InserirAsync(Revendedor revendedor);
        Task<Revendedor> ObterPorEmailAsync(string email);
    }
}
