using System;
using System.Dynamic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wasty.Models;

namespace wasty.Services
{
    public class AuthService : AuthModel
    {
        private readonly ApiService _apiService;
        private readonly SessionService _sessionService;

        public AuthService(ApiService apiService, SessionService sessionService)
        {
            _apiService = apiService;
            _sessionService = sessionService;
        }

        public async Task<bool> LoginAsync(string email, string contrasenia)
        {
            JsonElement dataElement = default;
            var login = new
            {
                email,
                contrasenia
            };

            var response = await _apiService.RequestAsync("POST", "auth/login", login);

            if (response is null || !response.exito)
            {
                return false;
            }
            else
            {
                var tokenResponse = JsonSerializer.Deserialize<Token>(response.datos);
                _sessionService.SaveToken(tokenResponse.tokenRefrendacion);
                return true; ;
            }
        }
        public void Logout()
        {
            _sessionService.ClearToken();
        }
    }
}