using Newtonsoft.Json.Linq;

namespace TodoApi.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public AdviceService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiUrl = config["AdviceApi:BaseUrl"];
        }

        public async Task<string> GetRandomAdvice()
        {
            var response = await _httpClient.GetAsync(_apiUrl);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to fetch advice.");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            return json["slip"]?["advice"]?.ToString() ?? "No advice found.";
        }
    }
}