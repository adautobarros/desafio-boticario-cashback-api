using MongoDB.Bson;
using System;

namespace Boticario.Cashback.Dominio.Entidades
{
    public abstract class EntidadeBase
    {
        public ObjectId Id { get; protected set; }
        public DateTimeOffset? DataCriacao { get; private set; }
        public DateTimeOffset? DataAtualizacao { get; protected set; }
        public EntidadeBase()
        {
            Id = ObjectId.GenerateNewId();
            DataCriacao = DateTimeOffset.UtcNow;
        }
    }
}
