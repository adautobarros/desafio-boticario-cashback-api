using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Extensions;
using Flunt.Br.Extensions;
using Flunt.Validations;
using MongoDB.Bson;
using System;

namespace Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands
{
    public class AtualizarCompraCommand : Request<Response>
    {
        public ObjectId Id { get; private set; }
        public string Codigo { get; set; }
        public decimal Valor { get; set; }
        public DateTimeOffset Data { get; set; }
        public string Cpf { get; set; }

        public AtualizarCompraCommand(string codigo,
            decimal valor,
            DateTimeOffset data,
            string cpf)
        {
            AddNotifications(new Contract()
                  .Requires()
                  .IsNotNullOrWhiteSpace(codigo, nameof(codigo), nameof(codigo).CampoObrigatorio())
                  .IsCpf(cpf, nameof(cpf), "Cpf inválido")
                  .IsGreaterThan(valor, 0, nameof(valor), nameof(valor).CampoObrigatorio())
                  .IsNotNull(data, nameof(data), nameof(data).CampoObrigatorio())
              );

            if (Invalid)
                return;

            Codigo = codigo;
            Valor = valor;
            Data = data;
            Cpf = cpf;
        }

        public void AdicionarId(string id)
        {
            Id = this.ObjectIdCast(id);
        }
    }
}
