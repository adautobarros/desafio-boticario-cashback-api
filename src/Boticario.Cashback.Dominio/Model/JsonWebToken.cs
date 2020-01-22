using Boticario.Cashback.Dominio.Entidades;

namespace Boticario.Cashback.Dominio.Model
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string TokenType { get; set; } = "bearer";
        public long ExpiresIn { get; set; }
    }
}
