using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Model;

namespace Boticario.Cashback.Dominio.Servicos
{
    public interface IJwtService
    {
        JsonWebToken CreateJsonWebToken(Revendedor usuario);
    }
}
