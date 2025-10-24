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
    // GENERELLT OM VIEWMODELS: Anger HUR DATA SKA VISAS och HUR ANVÄNDAREN INTERAGERAR MED DATA 
    // Behöver MANAGERS för att få kontakt med MODELS
    // Ska hantera inloggningsflöde, registrering och glömt lösenord
    // ANROPA USERMANAGER för autentisering och registrering 
    // Ska signalera till appen när användare är inloggad (via event) 

    // INSTRUKTIONER: SKA HANTERA INLOGGAD ANVÄNDARE, DEFINIERA CURRENT USER, samt METODER OCH KOMMANDON
    // FÖR INLOGGNING och UTLOGGNING, REGISTRERING 

    // OBS: VIEW MODELS ska inte känna till VIEWS => ingen kod för att t ex öppna eller stänga fönster här 
    // KOMMANDON OCH METODER - Samarbetar med UserManagerklassen i Managersmappen ("Frågar UserManager om mallar")
    public class UserManagerViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _username;
        private string _email;
        private string _password;
        private string _error;

        // PUBLIKA EGENSKAPER - inkl egenskaper för kommandon 
        public UserManagerViewModel(UserManager userManager) // Upprättar samarbete med UserManager
        {
            _userManager = userManager;
            // Definierar kommando för inloggning
            LoginCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
            //    // Definierar kommando för registrering
            //    RegisterCommand = new RelayCommand(execute => Register(), canExecute => IsRegistered());
            //    // Definierar kommando för lösenordsbyte
            //    ResetPasswordCommand = new RelayCommand(execute => ResetPassword(), canExecute => CanResetPassword());
        }

        // fort Publ Egensk med mera effektiv deklaration 
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        // DEFINIERAR KOMMANDON 
            // SKRIVA COMMANDS: 1. Definiera metoden/funktionen, 2. Definiera kommando mha RelayCommand (addCommand), 3. Koppla metoden till kommandot
        // LOG IN-KOMMANDO via ICommand i RelayCommandManager
        public ICommand LoginCommand { get; }

        // METOD för att kunna visa inloggningsstatus
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
  
        // METOD för inloggning 
        private void Login()
        {
            if (_userManager.Login(Username, Password)) // Kollar matchning genom att anropa metod i UserManager
                // Om inloggning lyckas anropas (invoke) EVENTET (definierat längst ner i denna fil) LogInSuccess 
                // ...som meddelar (Invoke - en metod) alla "prenumeranter", dvs. alla delar i appen som lyssnar på eventet.
                // ? = "Om OnLoginSuccess inte är null, kalla Invoke(); annars gör inget"
                // this = Referens till den aktuella instandsen av klass som gör anropet
                // System.EventArgs.Empty = standardargument när inga specifika data behöver skickas med evenetet 
                LogInSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = "Fel användarnamn eller lösenord.";
        }
        // REGISTER-KOMMANDO via ICommand i RelayCommandManager
        public ICommand RegisterCommand { get; }
        //// METOD för att kunna visa om registering lyckats - BEHÖVS VÄL INTE? 
        //private bool IsRegistered() =>
        //    ValidateEmailAddress()
        // METOD för registrering 
        //private void Register()
        //{
        //    if (_user.ValidateEmailAddress)
        //}
        // REGISTER-KOMMANDO via ICommand i RelayCommandManager
        public ICommand ResetPasswordCommand { get; }
        // METOD för registrering 
        // METOD för att kunna visa om lösenord ändrats
        private bool CanResetPassword() =>
            !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        private void ResetPassword()
        {

        }
        // EVENT som Login-fönstret "prenumererar" på
        // När login lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler LogInSuccess;

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}