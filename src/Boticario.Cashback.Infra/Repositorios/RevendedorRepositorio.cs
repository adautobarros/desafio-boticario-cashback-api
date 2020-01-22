using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Repositorios;
using Boticario.Cashback.Infra.Context;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Boticario.Cashback.Infra.Repositorios
{
    public class RevendedorRepositorio : IRevendedorRepositorio
    {
        protected MongoDbContext Context { get; }

        public RevendedorRepositorio(MongoDbContext context) => Context = context;


        public async Task<Revendedor> ObterPorEmailAsync(string email)
        {
            return await Context.ObterAsync(Builders<Revendedor>.Filter.Eq(c => c.Email, email));
        }

        public async Task InserirAsync(Revendedor revendedor)
        {
            await Context.InserirAsync(revendedor);
        }
    }
}