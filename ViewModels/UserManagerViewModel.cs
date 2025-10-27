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
        private string _email;
        private string _password;
        private string _passwordRepeat;
        private string _error;
        private string _pin; // Hittepå-pinkod för att återställa glömt lösenord 
        private string _selectedCountry; 

        // EVENT som Login-fönstret "prenumererar" på
        // När login lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler? LogInSuccess; // Make event nullable

        // EVENT som Login-fönstret "prenumererar" på
        // När knappen trycks, körs de metoder som är kopplade till detta event.
        public event System.EventHandler? SignUpSelected; // Make event nullable

        // EVENT som Registrerings-fönstret "prenumererar" på
        // När registrering lyckas, körs de metoder som är kopplade till detta event.
        public event System.EventHandler? RegistrationSuccess; // Make event nullable

        // EVENT som Reset Password-fönstret "prenumererar" på
        // När lösenordsåterställning lyckas, körs de metoder som är kopplade till detta event.
        public event System.EventHandler? ResetPasswordSuccess; // Make event nullable

        // PUBLIKA EGENSKAPER - inkl egenskaper för kommandon 
        public UserManagerViewModel(UserManager userManager) // Upprättar samarbete med UserManager
        {
            // Tilldelar värde/parameter
            _userManager = userManager;
            _username = string.Empty; // Initierar med tom string
            _email = string.Empty; // Initierar med tom string
            _password = string.Empty; // Initierar med tom string
            _passwordRepeat = string.Empty; // Initierar med tom string
            _error = string.Empty; // Initierar med tom string
            _pin = string.Empty; // Initierar med tom string
            _selectedCountry = string.Empty; // Initierar med tom string
            // Definierar kommando för inloggning
            LogInCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
            // Definierar kommando för att komma till registrering
            SignUpCommand = new RelayCommand(SignUpSelected);
            // Definierar kommando för registrering
            RegisterCommand = new RelayCommand(execute => Register(), canExecute => CanRegister());
            // Definierar kommando för lösenordsåterställning
            RequestPinCommand = new RelayCommand(execute => RequestPin(), canExecute => CanRequestPin());
            // Definierar kommando för att ändra lösenord
            ChangePasswordCommand = new RelayCommand(execute => ChangePassword(), canExecute => CanChangePassword());
        }
        // execute => SignUp(), canExecute => CanSignUp()

        // fort Publ Egensk med mera effektiv deklaration 
        public string UserName
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
        public string PasswordRepeat
        {
            get => _passwordRepeat;
            set { _passwordRepeat = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }
        public string Pin
        {
            get => _pin;
            set { _pin = value; OnPropertyChanged(); }
        }
        public string SelectedCountry
        {
            get => _selectedCountry;
            set { _selectedCountry = value; OnPropertyChanged(); }
        }

        // DEFINIERAR KOMMANDON 
        // SKRIVA COMMANDS: 1. Definiera metoden/funktionen, 2. Definiera kommando mha RelayCommand (addCommand), 3. Koppla metoden till kommandot
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

        // SIGNUP-KOMMANDO via ICommand in RelayCommandManager
        public ICommand SignUpCommand { get; }
        // METOD för att aktivera registreringssknapp
        private bool CanSignUp() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(PasswordRepeat);


        // METOD för att komma till registreringsfönstret
        private void SignUp()
        {
            if (SignUpSelected == true)
            // vilken funktion behövs här? 
        }

        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public ICommand RegisterCommand { get; }
        // METOD för att aktivera registreringssknapp
        private bool CanRegister() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(PasswordRepeat);

        // METOD för registeringskommando 
        private void Register()
        {
            // .... code to come... 
            if (Password != PasswordRepeat)
            {
                Error = "Lösenorden matchar inte";
                return; // Avsluta
            }
            // Instansiera nytt användarobjekt 
            var newUser = new User
            {
                UserName = UserName,
                EmailAddress = Email,
                Password = Password,
                DisplayName = UserName, // Kan senare eventuellt låta användaren välja eget visningsnamn
                Role = "Member",
                PinCode = "0000", // Lägga till randomisering i ett senare projekt? 
                Country = SelectedCountry
            };
            // Anropa Register-metoden i UserManager
            bool success = _userManager.Register(newUser);
            if (success)
            {
                // OM lyckad registering => Trigga event (öppna inloggnings-vyn) 
                RegistrationSuccess?.Invoke(this, System.EventArgs.Empty);
            }
            else
            {
                // ANNARS visa felmeddelande
                Error = "Användarnamn eller epost finns redan registrerad";
            }
        }

        // CHANGE PASSWORD-KOMMANDO via ICommand i RelayCommandManager
        public ICommand ChangePasswordCommand { get; }
        // METOD för att aktivera registreringssknapp - UPPDATERA
        private bool CanChangePassword() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(PasswordRepeat);

        // METOD för att ändra lösenord 
        private void ChangePassword()
        {
            // .... code to come... 
        }

        // TESTPIN-KOMMANDO via ICommand in RelayCommandManager
        public ICommand RequestPinCommand { get; }

        // METOD för att aktivera återställningssknapp - UPPDATERA
        private bool CanRequestPin() =>
            !string.IsNullOrWhiteSpace(Email);
        // METOD för att uppdatera glömt lösenord med hjälp av pinkod  - UPPDATERA
        public void RequestPin()
        {
            // .... code to come... 
        }

        // METOD för att validera användarnamn vid skapande av användare 
        public (bool success, string message) ValidateUsername(string username)
        {
            bool IsValidUsername = (username.Length > 3 && username.Length < 9) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidUsername ? "Användarnamnet har sparats" : "Användarnamnet ska vara mellan 4 och 8 bokstäver eller tecken långt";
            if (IsValidUsername)
            {
                string UserName = username;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera e-postadress vid skapande av användare 
        public (bool success, string message) ValidateEmailAddress(string email)
        {
            bool IsValidEmailAddress = (email.Contains("@") && email.IndexOf('.') > email.IndexOf('@')) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidEmailAddress ? "E-postadressen har sparats" : "E-postadressen är ogiltig. Försök igen.";
            if (IsValidEmailAddress)
            {
                string EmailAddress = email;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera lösenord vid skapande av användare eller vid byte av lösenord
        public (bool success, string message) ValidatePassword(string password, string passwordRepeat)
        {
            // villkor för att validera nytt lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            bool IsValidPassword = (password.Length > 3 && password.Length < 9 && password.Any(char.IsUpper) && password.Any(char.IsLower)
                    && password.Any(char.IsDigit) && password.Contains(specialCharacters)) ? true : false; // Ternary sats istället för IF-sats
            string message = IsValidPassword ? "Lösenordet har sparats" : "Lösenordet ska innehålla minst 4 och max 8 bokstäver inklusive ett specialtecken och en siffra";
            if (IsValidPassword)
            {

                bool IsPasswordMatch = (password == passwordRepeat) ? true : false; // Ternary sats istället för IF-sats
                message = IsPasswordMatch ? "Lösenordet har sparats" : "Lösenorden matchar inte varandra";
                if (IsPasswordMatch)
                {
                    string Password = passwordRepeat;
                    return (true, message);
                }
                else
                {
                    return (false, message);
                }
            }
            else
            {
                return (false, message);
            }
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}