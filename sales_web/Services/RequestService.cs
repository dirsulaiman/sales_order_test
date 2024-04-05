using System.Text;
using Microsoft.AspNetCore.WebUtilities;
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
        
        public async Task<T> Get<T>(string path, Dictionary<string, string> queryParams = null)
        {
            try
            {
                string endpoint = queryParams != null ? QueryHelpers.AddQueryString(path, queryParams) : path;
                
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
        
        public async Task Post(string path, object postData)
        {
            try
            {
                string endpoint = path;

                // Serialize the post data to JSON
                string jsonData = JsonConvert.SerializeObject(postData);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send POST request and read response
                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
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