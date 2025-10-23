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
    // FÖR INLOGGNING och UTLOGGNING

    // OBS: VIEW MODELS ska inte känna till VIEWS => ingen kod för att t ex öppna eller stänga fönster här 
    // KOMMANDON OCH METODER - Samarbetar med UserManagerklassen i Managersmappen ("Frågar UserManager om mallar")
    public class UserManagerViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _username;
        private string _password;
        private string _error;

        // PUBLIKA EGENSKAPER 
        public UserManagerViewModel(UserManager userManager)
        {
            _userManager = userManager;
            // Definierar kommando för inloggning
            LoginCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
        }
        // fort Publ Egensk med mera effektiv deklaration 
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
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
        // PUBLIK EGENSKAP för kommando via ICommand i RelayCommandManager
        public ICommand LoginCommand { get; }
        // PUBLIK EGENSKAP med villkor (tomma fält = inaktiverad knapp) 
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        // METOD för inloggning 
        private void Login()
        {
            if (_userManager.Login(Username, Password))
                //    Om inloggningen lyckas:
                //    → "OnLoginSuccess" är ett event
                //    → Invoke "talar" till alla som lyssnar på det eventet (i det här fallet: LoginWindow)
                //    → (this, EventArgs.Empty) skickar med en referens till vem som skickade eventet + tomma eventdata
                LogInSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = "Fel användarnamn eller lösenord.";
        }
        //public event Action<User>? LogInSuccess;
        //// Metod
        //private void LogInUser()
        //{
        //    //UserList.Add(new User { UserName = "...", Password = "...", });
        //}
        // CONSTRUCTOR 


        //        •	Kommandon:
        //o RelayCommand LogInCommand
        //o   RelayCommand RegisterCommand
        //o RelayCommand ForgotPasswordCommand
        //•	Event:

        //// METOD för att kunna visa inloggningsstatus
        //public bool IsAuthenticated => CurrentUser != null;

        //// Metod för att lägga till användare i lista 
        //private void CreateDefaultUsers()
        //{
        //    _userlist.Add(new User { Username = "LindaMaria", DisplayName = "Administratör", Role = "admin", Password = "0000" });
        //    //_userlist.Add(new User{Username = "Elsa", DisplayName = "Elsa", Role = "member", Password = "0001" });
        //    //_userlist.Add(new User{Username = "Elvis", DisplayName = "Elvis", Role = "member", Password = "0002" });
        //}

        //// METOD för att logga ut 
        //public void Logout()
        //{
        //    CurrentUser = null;
        //}
        public event System.EventHandler LogInSuccess; 

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
