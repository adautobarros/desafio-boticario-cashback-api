using MongoDB.Bson;
using System;

namespace Boticario.Cashback.Dominio.Entidades
{
    public class RefreshToken
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime DataExpiracao { get; set; }
    }
}
