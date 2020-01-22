using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Servicos
{
    public interface IBoticarioApiService
    {
        Task<decimal?> Cashback(string cpf);
    }
}
