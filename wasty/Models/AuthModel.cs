using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class AuthModel
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
