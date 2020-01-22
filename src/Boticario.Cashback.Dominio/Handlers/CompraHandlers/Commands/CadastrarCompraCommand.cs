using Boticario.Cashback.Dominio.Core;
using Boticario.Cashback.Dominio.Extensions;
using Flunt.Br.Extensions;
using Flunt.Validations;
using System;

namespace Boticario.Cashback.Dominio.Handlers.CompraHandlers.Commands
{
    public class CadastrarCompraCommand : Request<Response>
    {
        public string Codigo { get; set; }
        public decimal Valor { get; set; }
        public DateTimeOffset Data { get; set; }
        public string Cpf { get; set; }

        public CadastrarCompraCommand(string codigo,
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

            Codigo = codigo.Trim();
            Valor = valor;
            Data = data;
            Cpf = cpf.Trim().SomenteNumeros();
        }
    }
}
