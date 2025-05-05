using System.Net.Http;
using System.Text;
using System.Text.Json;
using wasty.Models;

namespace wasty.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly SessionService _sessionService;

        public ApiService(HttpClient httpClient, SessionService sessionService)
        {
            _httpClient = httpClient;
            _sessionService = sessionService;
        }

        public async Task<ApiResponse<dynamic>> RequestAsync<T>(string method, string endpoint, T data)
        {
            var response = new HttpResponseMessage();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var token = string.Empty;

            if (!endpoint.Contains("login"))
                token = await RefreshTokenAsync();

            // Agregar el token Bearer al encabezado de autorización si se proporciona
            if (!string.IsNullOrWhiteSpace(token))
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                if (method == "POST")
                    response = await _httpClient.PostAsync(endpoint, content);
                if (method == "GET")
                    response = await _httpClient.GetAsync(endpoint);

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<ApiResponse<dynamic>>(responseContent);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> RefreshTokenAsync()
        {
            var refreshToken = _sessionService.LoadToken();
            var json = JsonSerializer.Serialize(new { TokenRefrendacion = refreshToken });
            var response = await _httpClient.PostAsync("auth/refrendartoken", new StringContent(JsonSerializer.Serialize(new { TokenRefrendacion = refreshToken }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<dynamic>>(responseContent);
                var tokenResponse = JsonSerializer.Deserialize<Token>(apiResponse.datos);
                _sessionService.SaveToken(tokenResponse.tokenRefrendacion);
                return tokenResponse.token;
            }
            else
            {
                _sessionService.ClearToken();
            }
            return null;
        }
    }
}