using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Repositorios;
using Boticario.Cashback.Infra.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boticario.Cashback.Infra.Repositorios
{
    public class CompraRepositorio : ICompraRepositorio
    {
        protected MongoDbContext Context { get; }

        public CompraRepositorio(MongoDbContext context) => Context = context;

        public async Task<Compra> ObterPorId(ObjectId id)
        {
            return await Context.ObterAsync(Builders<Compra>.Filter.Eq(c => c.Id, id));
        }

        public async Task<Compra> ObterPorCodigo(string codigo)
        {
            return await Context.ObterAsync(Builders<Compra>.Filter.Eq(c => c.Codigo, codigo));
        }

        public async Task<ICollection<Compra>> ObterItensAsync()
        {
            return await Context.ObterItensAsync(Builders<Compra>.Filter.Empty, "codigo", "valor", "data", "porcentagemCashback", "valorCashback", "status");
        }

        public async Task InserirAsync(Compra compra)
        {
            await Context.InserirAsync(compra);
        }

        public async Task AtualizarAsync(Compra compra)
        {
            await Context.AtualizarAsync(compra.Id, compra);
        }

        public async Task ExcluirAsync(Compra compra)
        {
            await Context.ExcluirAsync<Compra>(compra.Id);
        }
    }
}