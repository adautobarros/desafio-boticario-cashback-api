using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boticario.Cashback.Infra.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(string connectionString, string nomeBanco)
        {
            if (string.IsNullOrWhiteSpace(nomeBanco) || string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("ConnectionString não informada ou nome do banco de dados não informado");
            }

            ConventionPackMongo.UseConventionMongo();
            _database = new MongoClient(connectionString).GetDatabase(nomeBanco);

            if (_database == null)
            {
                throw new Exception("Não foi possível conectar no mongodb");
            }
        }

        public IMongoCollection<T> ObterColecao<T>()
        {
            return _database.GetCollection<T>(ObterNomeColection<T>());
        }

        private string ObterNomeColection<T>()
        {
            var nomeColection = typeof(T).Name;
            return ToLowerInitLeter(nomeColection);
        }

        private string ToLowerInitLeter(string valor)
        {
            return $"{char.ToLowerInvariant(valor[0])}{valor.Substring(1)}";
        }

        public async Task InserirAsync<T>(T item)
        {
            await ObterColecao<T>().InsertOneAsync(item);
        }

        public async Task<T> AtualizarAsync<T>(ObjectId id, T item)
        {
            return await ObterColecao<T>().FindOneAndReplaceAsync(Builders<T>.Filter.Eq("_id", id), item);
        }

        public async Task<DeleteResult> ExcluirAsync<T>(ObjectId id)
        {
            return await ObterColecao<T>().DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }

        public async Task<T> ExcluirAsync<T>(FilterDefinition<T> filtro)
        {
            return await ObterColecao<T>().FindOneAndDeleteAsync<T>(filtro);
        }

        public async Task<T> ObterAsync<T>(FilterDefinition<T> filtro, params string[] colunas)
        {
            return await ObterColecao<T>()
                .Find(filtro)
                .Project<T>(ObterProjection<T>(colunas))
                .FirstOrDefaultAsync();
        }

        private ProjectionDefinition<T> ObterProjection<T>(params string[] colunas)
        {
            var projections = new List<ProjectionDefinition<T>>();
            if (colunas != null && colunas.Length > 0)
            {
                projections.AddRange(colunas.Select(item => Builders<T>.Projection.Include(item)));
            }

            return Builders<T>.Projection.Combine(projections.ToArray());
        }

        public async Task<ICollection<T>> ObterItensAsync<T>(FilterDefinition<T> where, params string[] colunas)
        {
            return await ObterColecao<T>()
                .Find(where)
                .Project<T>(ObterProjection<T>(colunas))
                .ToListAsync();
        }
    }
}
