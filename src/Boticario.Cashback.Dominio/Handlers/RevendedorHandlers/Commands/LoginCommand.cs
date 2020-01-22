using Boticario.Cashback.Dominio.Core;
using Flunt.Notifications;
using Flunt.Validations;

namespace Boticario.Cashback.Dominio.Handlers.RevendedorHandlers.Commands
{
    public class LoginCommand : Request<Response>
    {
        public string Email { get; }
        public string Senha { get; }
        public string GrantType { get; }
        public string RefreshToken { get; }

        public LoginCommand(string grantType, string email, string senha, string refreshToken)
        {
            Validate(grantType, email, senha, refreshToken);

            GrantType = grantType;
            Email = email;
            Senha = senha;
            RefreshToken = refreshToken;
        }

        private void Validate(string grantType, string email, string senha, string refreshToken)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(grantType, nameof(grantType), "O tipo de autenticação não pode ficar vazio"));

            if (!string.IsNullOrEmpty(grantType))
            {
                if (grantType.Equals("password"))
                {
                    AddNotifications(new Contract()
                        .Requires()
                        .IsEmail(email, nameof(email), "E-mail inválido")
                        .IsNotNullOrEmpty(senha, nameof(senha), "A senha não pode ficar vazia"));

                }
                else if (grantType.Equals("refresh_token"))
                {
                    AddNotifications(new Contract()
                        .Requires()
                        .IsNotNullOrEmpty(refreshToken, nameof(refreshToken), "O refresh token não pode ficar vazio"));
                }
                else
                {
                    AddNotification(new Notification(nameof(grantType), "Tipo de autenticação inválido"));
                }
            }
        }
    }
}
