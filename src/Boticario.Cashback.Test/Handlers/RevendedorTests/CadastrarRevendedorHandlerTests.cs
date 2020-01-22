using Boticario.Cashback.Dominio.Handlers.RevendedorHandlers;
using Boticario.Cashback.Dominio.Handlers.RevendedorHandlers.Commands;
using Boticario.Cashback.Test.Fakes;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Boticario.Cashback.Test.Handlers.RevendedorTests
{
    public class CadastrarRevendedorHandlerTests
    {
        [Fact]
        public async Task Deve_Criar_Revendedor_Quando_Pasado_Valores_Validos_Async()
        {
            var handler = new CadastrarRevendedorHandler(new FakeRevendedorRepositorio());
            var command = new CadastrarRevendedorCommand("Adauto Barros", "09230359661", "adauto@fake.com", "123456");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.False(response.Invalid);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Pasado_Cpf_Inalido_Async()
        {
            var handler = new CadastrarRevendedorHandler(new FakeRevendedorRepositorio());
            var command = new CadastrarRevendedorCommand("Adauto Barros", "092303519661", "adauto@fake.com", "123456");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.True(response.Invalid);
            Assert.Equal("Cpf inválido", response.Notifications.First().Message);
        }
    }
}
