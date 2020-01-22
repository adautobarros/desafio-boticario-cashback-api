using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Extensions;
using Flunt.Br.Extensions;
using Flunt.Validations;

namespace Boticario.Cashback.Dominio.Handlers.RevendedorHandlers.Commands
{

    public class CadastrarRevendedorCommand : Request<Response>
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public CadastrarRevendedorCommand(string nome,
            string cpf,
            string email,
            string senha)
        {
            AddNotifications(new Contract()
                  .Requires()
                  .IsNotNullOrWhiteSpace(nome, nameof(nome), nameof(nome).CampoObrigatorio())
                  .IsCpf(cpf, nameof(cpf), "Cpf inválido")
                  .IsNotNullOrWhiteSpace(email, nameof(email), nameof(email).CampoObrigatorio())
                  .IsEmail(email, nameof(email), "E-mail inválido")
                  .IsNotNullOrWhiteSpace(senha, nameof(senha), nameof(senha).CampoObrigatorio())
              );

            if (Invalid)
                return;

            Nome = nome.Trim();
            Cpf = cpf.Trim().SomenteNumeros();
            Email = email.Trim();
            Senha = senha;
        }
    }
}
