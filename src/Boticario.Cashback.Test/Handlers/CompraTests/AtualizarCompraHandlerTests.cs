using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands;
using Boticario.Cashback.Test.Fakes;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Boticario.Cashback.Test.Handlers.CompraTests
{
    public class AtualizarCompraHandlerTests
    {
        [Fact]
        public async Task Deve_Atualizar_Compra_Quando_Pasado_Compra_Valida_Async()
        {
            var repositorio = new FakeCompraRepositorio();
            var compra = new Compra("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "21330371011");
            repositorio.IncluirFake(compra);

            var handler = new AtualizarCompraHandler(repositorio);
            var command = new AtualizarCompraCommand("123", 2000, new DateTimeOffset(2020, 1, 22, 20, 0, 0, TimeSpan.Zero), "21330371011");
            command.AdicionarId(compra.Id.ToString());
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.False(response.Invalid);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Compra_Quando_Ela_Nao_Existe_Async()
        {
            var repositorio = new FakeCompraRepositorio();
            var compra = new Compra("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "21330371011");
            repositorio.IncluirFake(compra);


            var handler = new AtualizarCompraHandler(repositorio);
            var command = new AtualizarCompraCommand("123", 2000, new DateTimeOffset(2020, 1, 22, 20, 0, 0, TimeSpan.Zero), "21330371011");
            command.AdicionarId(ObjectId.GenerateNewId().ToString());
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.True(response.Invalid);
            Assert.Equal("Compra não encontrada", response.Notifications.First().Message);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Compra_Quando_Status_Compra_Aprovado_Async()
        {
            var repositorio = new FakeCompraRepositorio();
            var compra = new Compra("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "15350946056");
            repositorio.IncluirFake(compra);


            var handler = new AtualizarCompraHandler(repositorio);
            var command = new AtualizarCompraCommand("123", 2000, new DateTimeOffset(2020, 1, 22, 20, 0, 0, TimeSpan.Zero), "15350946056");
            command.AdicionarId(compra.Id.ToString());
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.True(response.Invalid);
            Assert.Equal("Compras aprovadas não podem ser editadas", response.Notifications.First().Message);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Compra_Quando_Codigo_Ja_Existe_Async()
        {
            var repositorio = new FakeCompraRepositorio();
            var compra = new Compra("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "21330371011");
            repositorio.IncluirFake(compra);

            var compra2 = new Compra("1234", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "21330371011");
            repositorio.IncluirFake(compra2);


            var handler = new AtualizarCompraHandler(repositorio);
            var command = new AtualizarCompraCommand("1234", 2000, new DateTimeOffset(2020, 1, 22, 20, 0, 0, TimeSpan.Zero), "21330371011");
            command.AdicionarId(compra.Id.ToString());
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.True(response.Invalid);
            Assert.Equal("Jà existe uma Compra com esse código", response.Notifications.First().Message);
        }
    }
}
