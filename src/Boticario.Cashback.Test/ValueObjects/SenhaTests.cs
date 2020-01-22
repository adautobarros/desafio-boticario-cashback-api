using Boticario.Cashback.Dominio.ValueObjects;
using Xunit;

namespace Boticario.Cashback.Test.ValueObjects
{
    public class SenhaTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        public void Deve_Retornar_Erro_Quando_Nao_Pasado_Valor_Valido_Para_Senha(string senha)
        {
            Assert.True(new Senha(senha).Invalid);
        }

        [Fact]
        public void Deve_Retornar_Senha_Quando_Pasado_Valor_Valido_Para_Senha()
        {
            var senha = new Senha("123456");
            Assert.True(senha.Valid);
            Assert.NotNull(senha);
        }
    }
}
