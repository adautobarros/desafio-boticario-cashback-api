using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Repositorios;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boticario.Cashback.Test.Fakes
{
    internal class FakeCompraRepositorio : ICompraRepositorio
    {
        public List<Compra> compras = new List<Compra>();

        public void IncluirFake(Compra compra)
        {
            compras.Add(compra);
        }

        public async Task AtualizarAsync(Compra compra)
        {
            var item = compras.FirstOrDefault(c => c.Codigo == compra.Codigo);
            if (item == null)
                compras.Add(compra);
            else
            {
                item.Atualizar(compra.Codigo, compra.Valor, compra.Data, compra.Cpf);
            }

            await Task.CompletedTask;
        }

        public async Task ExcluirAsync(Compra compra)
        {
            compras.Remove(compra);
            await Task.CompletedTask;
        }

        public async Task InserirAsync(Compra compra)
        {
            compras.Add(compra);
            await Task.CompletedTask;
        }

        public async Task<ICollection<Compra>> ObterItensAsync()
        {
            await Task.CompletedTask;
            return compras;
        }

        public async Task<Compra> ObterPorCodigo(string codigo)
        {
            await Task.CompletedTask;
            return compras.FirstOrDefault(c => c.Codigo == codigo);
        }

        public async Task<Compra> ObterPorId(ObjectId id)
        {
            await Task.CompletedTask;
            return compras.FirstOrDefault(c => c.Id == id);
        }
    }
}
