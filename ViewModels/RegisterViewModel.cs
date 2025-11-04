using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.Views;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _newUsername;
        private string _newPassword;
        private string _newSelectedCountry;
        private string _error;

        // PUBLIKA EGENSKAPER med mera effektiv deklaration 
        public string NewUserName { get => _newUsername;
            set { _newUsername = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string NewPassword { get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }

        // Koppla till listan över länder i UserManager så att den ("ItemsSource" i View) 
        public List<string> Countries => _userManager.Countries;

        public string NewSelectedCountry
        { get => _newSelectedCountry; set { _newSelectedCountry = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error
        { get => _error; set { _error = value; OnPropertyChanged(); }
        }

        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public RelayCommand RegisterCommand => new RelayCommand(execute => Register(), canExecute => CanRegister());

        // METOD för att aktivera registreringssknapp
        private bool CanRegister() =>
            !string.IsNullOrWhiteSpace(NewUserName) 
            && !string.IsNullOrWhiteSpace(NewPassword) 
            && !string.IsNullOrWhiteSpace(NewSelectedCountry);

        // KONSTRUKTOR som upprättar samarbete med UserManager 
        public RegisterViewModel()
        {
            // Tilldelar värden för samarbete m globala variabler
            _userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Tilldelar värde/parameter
            _newUsername = string.Empty; // Säkrare hantering av data 
            _newPassword = string.Empty; 
            _newSelectedCountry = string.Empty;
            _error = string.Empty;
        }
        // FÖNSTERSTÄNGARE 
        private void CloseWindow<T>() where T : Window
        {
            var win = Application.Current.Windows.OfType<T>().FirstOrDefault();
            win?.Close();
        }
        // METOD för KOMMANDO
        public void Register()
        {
            // Anropar Registrerings-metod i UserManager
            var (success, message) = _userManager.Register(NewUserName, NewPassword, NewSelectedCountry);
            // Kollar matchning genom att anropa metod i UserManager
            if (success) 
                RegisterSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = message; // Tar meddelanden från UserManager 
            //// Anropar fönsterstängare
            //CloseWindow<RegisterWindow>();
            //// Öppnar inloggningsvyn igen
            ////var mainVM = new MainViewModel();
            //var mainWindow = new MainWindow();
            //mainWindow.Show();
        }

        // EVENT som Registrerings-fönstret "prenumererar" på
        public event System.EventHandler? RegisterSuccess; 

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}