using Meme.Hub.TokenRawDataProcessor.Interfaces;
using Newtonsoft.Json;

namespace Meme.Hub.TokenRawDataProcessor.Services
{

    public class DataHttpClient: IDataHttpClient
    {
        private readonly HttpClient _httpClient;

        public DataHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetData<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }
    }
}
