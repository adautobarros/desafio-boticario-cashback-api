using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boticario.Cashback.Test.Fakes
{
    internal class FakeRevendedorRepositorio : IRevendedorRepositorio
    {
        public List<Revendedor> revendedores = new List<Revendedor>();
        public async Task InserirAsync(Revendedor revendedor)
        {
            revendedores.Add(revendedor);
            await Task.CompletedTask;
        }

        public async Task<Revendedor> ObterPorEmailAsync(string email)
        {
            if (email == "teste@fake.com")
                return new Revendedor("teste", "123456789", "teste@fake.com", "123456");

            await Task.CompletedTask;
            return null;
        }
    }
}
