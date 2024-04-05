using Newtonsoft.Json;

namespace SalesOrderWeb.Services
{
    public class RequestService
    {
        private readonly HttpClient _httpClient;

        public RequestService(IConfiguration configuration)
        {
            // Initialize HttpClient with base address
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("BaseSalesApi"));
        }
        
        public async Task<T> Get<T>(string path)
        {
            try
            {
                string endpoint = path;
                // Send GET request and read response
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read content as string
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response data
                    T data = JsonConvert.DeserializeObject<T>(responseData);
                    return data;
                }
                else
                {
                    // Handle error
                    throw new Exception("Error: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}