using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
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
        private string _password;
        private string _error;

        // EVENT som Login-fönstret "prenumererar" på
        // När login lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler? LogInSuccess; // Make event nullable

        // PUBLIKA EGENSKAPER - inkl egenskaper för kommandon 
        public UserManagerViewModel(UserManager userManager) // Upprättar samarbete med UserManager
        {
            // Tilldelar värde/parameter
            _userManager = userManager;
            _username = string.Empty; // Initialize to empty string
            _password = string.Empty; // Initialize to empty string
            _error = string.Empty;    // Initialize to empty string

            // Definierar kommando för inloggning
            LogInCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
            //    // Definierar kommando för registrering
            //    RegisterCommand = new RelayCommand(execute => Register(), canExecute => IsRegistered());
            //    // Definierar kommando för lösenordsbyte
            //    ResetPasswordCommand = new RelayCommand(execute => ResetPassword(), canExecute => CanResetPassword());
        }

        // fort Publ Egensk med mera effektiv deklaration 
        public string UserName
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        //public string Email
        //{
        //    get => _email;
        //    set { _email = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        //}
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
        public ICommand LogInCommand { get; }

        // METOD för att aktivera inloggningsknapp
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
  
        // METOD för inloggning 
        private void Login()
        {
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
        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public ICommand RegisterCommand { get; }
        // METOD för registering 
        private void Register()
        {

            if (_userManager.ValidateUsername(UserName) && 
                _userManager.ValidatePassword(Password) && 
                _userManager.ValidateEmailAddress(EmailAddress) && 
                -userManager.ValidateRepeatedPassWord(PasswordRepeat)) 

            if ()
        }

        // REGISTER-KOMMANDO via ICommand i RelayCommandManager
        public ICommand ResetPasswordCommand { get; }

        // METOD för att ändra lösenord 
        private void ResetPassword()
        {

        }

        // TESTPIN-KOMMANDO via ICommand in RelayCommandManager
        public ICommand TestPinCommand { get; }

        // METOD för att uppdatera glömt lösenord med hjälp av pinkod
        public void TestPin()
        {

        }


        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}