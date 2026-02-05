namespace ElasticSerilog.Clients
{
    public class ChuckNorrisClient
    {
        protected readonly HttpClient _httpClient;
        public ChuckNorrisClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public virtual async Task<string> GetJokeAsync()
        {
            var clientResponse = await _httpClient.GetAsync("jokes/random");
                return await clientResponse.Content.ReadAsStringAsync();
        }
    }
}
