using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Services;
using WpfApp1.Views;

namespace WpfApp1.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;
        private string _login;
        private string _password;
        private Employee _authenticatedEmployee;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }
        public bool IsLoggedIn { get; private set; }

        public LoginViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoginCommand = new RelayCommand(Authenticate);
        }

        private void Authenticate()
        {
            _authenticatedEmployee = _databaseService.Authenticate(Login, Password);

            if (_authenticatedEmployee != null)
            {
                IsLoggedIn = true;
                var catalogView = new CatalogView();
                catalogView.Show();
                (this.GetVisualAncestor<Window>() as Window)?.Close();
            }
            else
            {
                MessageBox.Show("Пароль неверный");
            }
        }

        private T GetVisualAncestor<T>()
        {
            throw new NotImplementedException();
        }
    }
}
