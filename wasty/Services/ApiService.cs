using System;
using System.Net.Http;
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

        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            // Leer el contenido de la respuesta como una cadena
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON en un objeto ApiResponse<TResponse>
            var result = JsonSerializer.Deserialize<ApiResponse<TResponse>>(responseContent);

            return result;
        }
    }
}