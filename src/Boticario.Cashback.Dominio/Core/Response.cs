using Flunt.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace Boticario.Cashback.Dominio.Core
{
    public class Response
    {
        private List<Notification> _notifications { get; } = new List<Notification>();

        public IReadOnlyCollection<Notification> Notifications => _notifications.ToList();

        public bool Invalid => _notifications.Any();

        public object Value { get; private set; }

        /// <summary>
        /// Cria um novo objeto de retorno para a api
        /// </summary>
        public Response()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="object">Objeto que deverá ser serializado pela Api</param>
        public Response(object @object) : this()
        {
            AddValue(@object);
        }

        /// <summary>
        /// Adiciona um objeto que deverá ser serializado e retornado pela Api
        /// </summary>
        /// <param name="object">Objeto que deverá ser serializado pela Api</param>
        public void AddValue(object @object)
        {
            Value = @object;
        }

        /// <summary>
        /// Adiciona message de retorno
        /// </summary>
        /// <param name="">message que deverá ser retornada pela Api</param>
        public void AddNotification(string message)
        {
            _notifications.Add(new Notification(null, message));
        }

        public void AddNotifications(IEnumerable<Notification> notificacoes)
        {
            _notifications.AddRange(notificacoes);
        }
    }
}
