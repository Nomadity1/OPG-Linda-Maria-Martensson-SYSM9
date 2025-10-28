using CookMaster.Managers;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CookMaster.ViewModels
{
    class LogInViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _username;
        private string _password;
        private string _email;
        private string _pin; // Hittepå-pinkod för att återställa glömt lösenord 
        private string _error;

        // KONSTRUKTOR 
        public LogInViewModel(UserManager userManager)
        {
            // Tilldelar värde/parameter
            _userManager = userManager;
            _username = string.Empty; // Initierar med tom string
            _email = string.Empty; // Initierar med tom string
            _password = string.Empty; // Initierar med tom string
            _error = string.Empty; // Initierar med tom string
            _pin = string.Empty; // Initierar med tom string

            // Anropar RelayCommand för inloggningskommando
            LogInCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
        }

        // PUBLIKA EGENSKAPER med mera effektiv deklaration 
        public string UserName
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Pin
        {
            get => _pin;
            set { _pin = value; OnPropertyChanged(); }
        }
        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        // DEFINIERAR KOMMANDON 
        // LOG IN-KOMMANDO via ICommand i RelayCommandManager
        public ICommand LogInCommand { get; }

        // METOD för att aktivera inloggningsknapp
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);

        // METOD för inloggningskommando
        private void Login()
        {
            // Anropar Login-metod i UserManager
            if (_userManager.Login(UserName, Password)) // Kollar matchning genom att anropa metod i UserManager
                // Om inloggning lyckas anropas (invoke) EVENTET (definierat längst ner i denna fil) LogInSuccess 
                // ...som meddelar (Invoke - en metod) alla "prenumeranter", dvs. alla delar i appen som lyssnar på eventet.
                // ? = "Om OnLoginSuccess inte är null, kalla Invoke(); annars gör inget"
                // this = Referens till den aktuella instandsen av klass som gör anropet
                // System.EventArgs.Empty = standardargument när inga specifika data behöver skickas med evenetet 
                LogInSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = "Fel användarnamn eller lösenord";
        }

        // EVENT som Login-fönstret "prenumererar" på
        // När login lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler? LogInSuccess; // Make event nullable

        // EVENT som Login-fönstret "prenumererar" på
        // När knappen trycks, körs de metoder som är kopplade till detta event.
        public event System.EventHandler? SignUpSelected; // Make event nullable

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
