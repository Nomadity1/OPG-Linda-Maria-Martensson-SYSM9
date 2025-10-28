using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    class RegisterViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _newUsername;
        private string _newPassword;
        private string _repeatPassword;
        private string _email;
        private string _selectedCountry;
        private string _error;

        // KONSTRUKTOR 
        public RegisterViewModel(UserManager userManager)
        {
            // Tilldelar värde/parameter
            _userManager = userManager;
            _newUsername = string.Empty; // Initierar med tom string
            _email = string.Empty; // Initierar med tom string
            _newPassword = string.Empty; // Initierar med tom string
            _repeatPassword = string.Empty; // Initierar med tom string
            _selectedCountry = string.Empty; // Initierar med tom string
            _error = string.Empty; // Initierar med tom string

            // Anropar RelayCommand för inloggningskommando
            RegisterCommand = new RelayCommand(execute => Register(), 
                canExecute => CanRegister());
        }

        // PUBLIKA EGENSKAPER med mera effektiv deklaration 
        public string NewUserName
        {
            get => _newUsername;
            set { _newUsername = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string RepeatPassword
        {
            get => _repeatPassword;
            set { _repeatPassword = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }
        // Koppla till listan över länder i UserManager så att den
        // kan fugnera som "ItemsSource" i View 
        public List<string> Countries => _userManager.Countries;

        public string SelectedCountry
        {
            get => _selectedCountry;
            set { _selectedCountry = value; OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public ICommand RegisterCommand { get; }

        // METOD för att aktivera registreringssknapp
        private bool CanRegister() =>
            !string.IsNullOrWhiteSpace(NewUserName) 
            && !string.IsNullOrWhiteSpace(Email) 
            && !string.IsNullOrWhiteSpace(NewPassword) 
            && !string.IsNullOrWhiteSpace(RepeatPassword) 
            && !string.IsNullOrWhiteSpace(SelectedCountry);

        // METOD för registeringskommando 
        public void Register()
        {
            // Anropar Registrerings-metod i UserManager
            var (success, message) = _userManager.Register(NewUserName, Email, NewPassword, RepeatPassword, SelectedCountry); 
            // Kollar matchning genom att anropa metod i UserManager
                // OM inloggning lyckas anropas (invoke) EVENTET (definierat längst ner i denna fil) LogInSuccess 
                // ...som meddelar (Invoke - en metod) alla "prenumeranter", dvs. alla delar i appen som lyssnar på eventet.
                // ? = "Om OnLoginSuccess inte är null, kalla Invoke(); annars gör inget"
                // this = Referens till den aktuella instansen av klass som gör anropet
                // System.EventArgs.Empty = standardargument när inga specifika data behöver skickas med evenetet 
            if (success) 
                RegisterSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = message; // Tar meddelanden från UserManager 
        }
        // EVENT som Registrerings-fönstret "prenumererar" på
        // När registrering lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler? RegisterSuccess; // Make event nullable

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
