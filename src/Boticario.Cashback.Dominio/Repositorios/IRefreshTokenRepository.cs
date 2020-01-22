using Boticario.Cashback.Dominio.Entidades;
using System.Threading.Tasks;

namespace Boticario.Cashback.Dominio.Repositorios
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> Obter(string refreshToken);
        Task Salvar(RefreshToken refreshToken);
    }
}
