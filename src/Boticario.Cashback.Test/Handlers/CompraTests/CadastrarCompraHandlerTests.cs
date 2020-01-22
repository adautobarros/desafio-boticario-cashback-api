using Boticario.Cashback.Dominio.Entidades;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers;
using Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands;
using Boticario.Cashback.Test.Fakes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Boticario.Cashback.Test.Handlers.CompraTests
{
    public class CadastrarCompraHandlerTests
    {
        [Fact]
        public async Task Deve_Cadastrar_Compra_Quando_Pasado_Valores_Validos_Async()
        {
            var handler = new CadastrarCompraHandler(new FakeCompraRepositorio());
            var command = new CadastrarCompraCommand("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "09230359661");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.False(response.Invalid);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Pasado_Cpf_Inalido_Async()
        {
            var handler = new CadastrarCompraHandler(new FakeCompraRepositorio());
            var command = new CadastrarCompraCommand("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "09230356661");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.True(response.Invalid);
            Assert.Equal("Cpf inválido", response.Notifications.First().Message);
        }

        [Fact]
        public async Task Deve_Cadastrar_Compra_Aprovada_Quando_Informado_Cpf_15350946056_Async()
        {
            var repositorio = new FakeCompraRepositorio();
            var handler = new CadastrarCompraHandler(repositorio);
            var command = new CadastrarCompraCommand("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "15350946056");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            var compra = await repositorio.ObterPorCodigo("123");

            Assert.False(response.Invalid);
            Assert.Equal(StatusCompra.Aprovado, compra.Status);

        }

        [Theory]
        [InlineData("60317114000")]
        [InlineData("73041621045")]
        [InlineData("58811621020")]
        public async Task Deve_Cadastrar_Compra_EmValidacao_Quando_Informado_Cpf_Diferente_De_15350946056_Async(string cpf)
        {
            var repositorio = new FakeCompraRepositorio();
            var handler = new CadastrarCompraHandler(repositorio);
            var command = new CadastrarCompraCommand("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), cpf);
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            var compra = await repositorio.ObterPorCodigo("123");

            Assert.False(response.Invalid);
            Assert.Equal(StatusCompra.EmValidacao, compra.Status);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(999)]
        [InlineData(10.50)]
        public async Task Deve_Cadastrar_Compra_Com_10_Porcento_Quando_Informado_Valores_Menores_Que_1000_Async(decimal valor)
        {
            var repositorio = new FakeCompraRepositorio();
            var handler = new CadastrarCompraHandler(repositorio);
            var command = new CadastrarCompraCommand("123", valor, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "73041621045");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            var compra = await repositorio.ObterPorCodigo("123");

            byte porcentagemEsperada = 10;
            Assert.False(response.Invalid);
            Assert.Equal(porcentagemEsperada, compra.PorcentagemCashback);
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(1200.50)]
        [InlineData(1500)]
        public async Task Deve_Cadastrar_Compra_Com_15_Porcento_Quando_Informado_Valores_Entre_1000_E_1500_Async(decimal valor)
        {
            var repositorio = new FakeCompraRepositorio();
            var handler = new CadastrarCompraHandler(repositorio);
            var command = new CadastrarCompraCommand("123", valor, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "73041621045");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            var compra = await repositorio.ObterPorCodigo("123");

            byte porcentagemEsperada = 15;
            Assert.False(response.Invalid);
            Assert.Equal(porcentagemEsperada, compra.PorcentagemCashback);
        }

        [Theory]
        [InlineData(1500.01)]
        [InlineData(2000)]
        [InlineData(10000)]
        public async Task Deve_Cadastrar_Compra_Com_20_Porcento_Quando_Informado_Valores_Maiores_Que_1500_Async(decimal valor)
        {
            var repositorio = new FakeCompraRepositorio();
            var handler = new CadastrarCompraHandler(repositorio);
            var command = new CadastrarCompraCommand("123", valor, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "73041621045");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            var compra = await repositorio.ObterPorCodigo("123");

            byte porcentagemEsperada = 20;
            Assert.False(response.Invalid);
            Assert.Equal(porcentagemEsperada, compra.PorcentagemCashback);
        }

        [Fact]
        public async Task Deve_Cadastrar_Compra_Com_Cashback_De_50_Async()
        {
            var repositorio = new FakeCompraRepositorio();
            var handler = new CadastrarCompraHandler(repositorio);
            var command = new CadastrarCompraCommand("123", 500, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "73041621045");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            var compra = await repositorio.ObterPorCodigo("123");

            byte porcentagemEsperada = 10;
            decimal valorCashbackEsperada = 50;
            Assert.False(response.Invalid);
            Assert.Equal(porcentagemEsperada, compra.PorcentagemCashback);
            Assert.Equal(valorCashbackEsperada, compra.ValorCashback);
        }

        [Fact]
        public async Task Nao_Deve_Cadastrar_Compra_Quando_Codigo_Ja_Existe_Async()
        {
            var repositorio = new FakeCompraRepositorio();
            var compra = new Compra("123", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "21330371011");
            repositorio.IncluirFake(compra);

            var compra2 = new Compra("1234", 1000, new DateTimeOffset(2020, 1, 21, 20, 0, 0, TimeSpan.Zero), "21330371011");
            repositorio.IncluirFake(compra2);


            var handler = new CadastrarCompraHandler(repositorio);
            var command = new CadastrarCompraCommand("1234", 2000, new DateTimeOffset(2020, 1, 22, 20, 0, 0, TimeSpan.Zero), "21330371011");
            var response = await handler.Handle(command, System.Threading.CancellationToken.None);

            Assert.True(response.Invalid);
            Assert.Equal("Jà existe uma Compra com esse código", response.Notifications.First().Message);
        }
    }
}
