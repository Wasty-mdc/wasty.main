using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wasty.Models;

namespace wasty.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<dynamic> RequestAsync<T>(string method, string endpoint, T data, string token = "")
        {
            var r = JsonSerializer.Serialize(new ApiResponse<T>());
            var response = new HttpResponseMessage();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Agregar el token Bearer al encabezado de autorización si se proporciona
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                if (method == "POST")
                    response = await _httpClient.PostAsync(endpoint, content);
                if (method == "GET")
                    response = await _httpClient.GetAsync(endpoint);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent))
                    responseContent = r;

                JsonDocument document = JsonDocument.Parse(responseContent);
                JsonElement root = document.RootElement;

                return root;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}