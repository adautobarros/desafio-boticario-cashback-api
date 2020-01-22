using Boticario.Cashback.Dominio.ValueObjects;

namespace Boticario.Cashback.Dominio.Entidades
{
    public sealed class Revendedor : EntidadeBase
    {
        public Revendedor(string nome, string cpf, string email, string senha) : base()
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            Senha = new Senha(senha);
        }

        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
    }
}
