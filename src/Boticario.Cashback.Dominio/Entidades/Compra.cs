using System;

namespace Boticario.Cashback.Dominio.Entidades
{
    public enum StatusCompra
    {
        EmValidacao = 1,
        Aprovado = 2
    }
    public sealed class Compra : EntidadeBase
    {
        public Compra(string codigo, decimal valor, DateTimeOffset dataCompra, string cpf) : base()
        {
            Codigo = codigo;
            Valor = valor;
            Data = dataCompra;
            Cpf = cpf;

            AplicarStatus();
            AplicarCashback();
        }

        public string Codigo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTimeOffset Data { get; private set; }
        public string Cpf { get; private set; }
        public StatusCompra Status { get; private set; }
        public byte PorcentagemCashback { get; set; }
        public decimal ValorCashback { get; set; }
        private void AplicarCashback()
        {
            if (Valor < 1000M)
                PorcentagemCashback = 10;
            else if (Valor <= 1500M)
                PorcentagemCashback = 15;
            else
                PorcentagemCashback = 20;

            ValorCashback = Math.Round(((decimal)PorcentagemCashback / 100) * Valor, 2);
        }

        private void AplicarStatus()
        {
            if (Cpf == "15350946056")
                Status = StatusCompra.Aprovado;
            else
                Status = StatusCompra.EmValidacao;
        }

        public void Atualizar(string codigo, decimal valor, DateTimeOffset data, string cpf)
        {
            Codigo = codigo;
            Valor = valor;
            Data = data;
            Cpf = cpf;
            DataAtualizacao = DateTimeOffset.UtcNow;

            AplicarStatus();
            AplicarCashback();
        }
    }
}
