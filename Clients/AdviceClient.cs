namespace ElasticSerilog.Clients
{
    public class AdviceClient
    {
        protected readonly HttpClient _httpClient;
        public AdviceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public virtual async Task<string> GetAdviceAsync()
        {
            var clientResponse = await _httpClient.GetAsync("advice");
                return await clientResponse.Content.ReadAsStringAsync();
        }
    }
}
