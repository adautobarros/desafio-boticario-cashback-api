using Boticario.Cashback.Dominio.Model;
using Boticario.Cashback.Dominio.Servicos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Boticario.Cashback.Infra.Servicos
{
    public class BoticarioApiService : IBoticarioApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BoticarioApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<decimal?> Cashback(string cpf)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"v1/cashback?cpf={cpf}");

            var client = _httpClientFactory.CreateClient("boticario");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var cashBackResponse = JsonConvert.DeserializeObject<CashBackApiResponse>(responseString);

                return cashBackResponse.Body.Credit;
            }
            return null;
        }
    }
}
