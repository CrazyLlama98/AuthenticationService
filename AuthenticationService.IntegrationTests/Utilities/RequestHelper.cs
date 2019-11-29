using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.IntegrationTests.Utilities
{
    public class RequestHelper
    {
        private readonly HttpClient _httpClient;

        public RequestHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PostObject(string url, object body)
        {
            return await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
        }
    }
}
