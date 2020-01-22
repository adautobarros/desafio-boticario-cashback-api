using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Repositorios;
using Boticario.Cashback.Infra.Context;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Boticario.Cashback.Infra.Repositorios
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly MongoDbContext _context;

        public RefreshTokenRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> Obter(string refreshToken)
        {
            return await _context.ObterAsync(Builders<RefreshToken>.Filter.Eq(c => c.Token, refreshToken));
        }

        public async Task Salvar(RefreshToken refreshToken)
        {
            await _context.ExcluirAsync(Builders<RefreshToken>.Filter.Eq(c => c.Email, refreshToken.Email));
            await _context.InserirAsync(refreshToken);
        }
    }
}