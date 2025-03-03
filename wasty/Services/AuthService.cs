using System;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wasty.Models;

namespace wasty.Services
{
    public class AuthService
    {
        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnAuthenticationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnAuthenticationChanged;
    }
}