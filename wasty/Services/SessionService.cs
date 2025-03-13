using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wasty.Helpers;
using wasty.Models;
using wasty.Views;

namespace wasty.Services
{
    public class SessionService
    {
        private readonly NavigationService _navigationService;
        private readonly AuthModel _authModel;
        private readonly IsolatedStorageHelper _storageHelper;
        private const string AccessFileName = "access.wasty";

        public SessionService(IsolatedStorageHelper storageHelper, AuthModel authModel, NavigationService navigationService)
        {
            _storageHelper = storageHelper;
            _authModel = authModel;
            _navigationService = navigationService;
        }

        public void SaveToken(string refreshToken)
        {
            _authModel.IsAuthenticated = true;
            _storageHelper.SaveData(AccessFileName, refreshToken);
        }

        public string LoadToken()
        {
            var refreshToken = _storageHelper.LoadData(AccessFileName);
            return refreshToken;
        }

        public void ClearToken()
        {
            _authModel.IsAuthenticated = false;
            _storageHelper.SaveData(AccessFileName, string.Empty);
            _navigationService.NavigateTo<LoginView>();
        }
    }
}