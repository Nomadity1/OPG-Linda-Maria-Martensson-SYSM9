using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.ViewModels;
using CookMaster.Views;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    //HUVUDSAKLIG VIEWMODEL för HUVUDFÖNSTRET (MainWindow)
    //UPPGIFTER: visa inloggad användare, visa receptlista, sök- och filtreringsfunktion
    public class MainViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _username;
        private string _password;
        private string _answer;
        private string _error;
        //private readonly RecipeManager _recipeManager; SÅ SMÅNINGOM!! 

        // PUBLIK EGENSKAP som använder sig av UserManager-klassen (skapar förutsättning för samarbete med UserManager)
        public UserManager UserManager { get; }

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
        public string Answer
        {
            get => _answer;
            set { _answer = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        // KONSTRUKTOR med villkorssats för att visa login-fönster och användare 
        public MainViewModel() // Upprättar samarbete mln Main och UserManager
        {
            _userManager = new UserManager();
            //_recipeManager = new RecipeManager(); SENARE!
        }

        // PUBLIKA METOD-DEFINITIONER I LAMBDAUTTRYCK (EFFEKTIV FORM) som använder basklass RelayCommand)
        // FÖR INLOGGNING
        public RelayCommand LogInCommand => new RelayCommand(execute => Login(), canExecute => CanLogin());

        // FÖR ATT ÖPPNA REGISTRERINGSVYN
        public RelayCommand OpenRegisterCommand => new RelayCommand(execute => OpenRegister(), canExecute => CanOpenRegister());

        // FÖR LÖSENORDSÅTERSTÄLLNINGSKNAPP 
        public RelayCommand ResetPasswordCommand => new RelayCommand(execute => ResetPassword(), canExecute => CanResetPassword());


        // METODER för att aktivera knappar
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        private bool CanOpenRegister() => true; // Alltid aktiv
        private bool CanResetPassword() =>
            !string.IsNullOrWhiteSpace(UserName) 
            && !string.IsNullOrWhiteSpace(Answer);

        // METOD för inloggningskommando
        private void Login()
        {
            // Anropar ValidateLogInmetod i UserManager,
            // Har bytt namn på metod för att förtydliga att metoden i UserManager
            // är en annan metod än denna vi är i nu
            if (_userManager.ValidateLogin(UserName, Password)) // Kollar matchning genom att anropa metod i UserManager
                // Om inloggning lyckas anropas (invoke) EVENTET (definierat längst ner i denna fil) LogInSuccess 
                // ...som meddelar (Invoke - en metod) alla "prenumeranter", dvs. alla delar i appen som lyssnar på eventet.
                // ? = "Om OnLoginSuccess inte är null, kalla Invoke(); annars gör inget"
                // this = Referens till den aktuella instandsen av klass som gör anropet
                // System.EventArgs.Empty = standardargument när inga specifika data behöver skickas med eventet 
                LogInSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = "Fel användarnamn eller lösenord";
        }

        // METOD för kommando att öppna registrering
        public void OpenRegister()
        {
            // Instansierar registreringsvyn
            var registerWindow = new RegisterWindow();
            // ...och visar den 
            registerWindow.Show();
        }
        public void ResetPassword()
        {
            // Instansierar userdetails-fönster
            UserDetailsWindow userDetailsWindow = new UserDetailsWindow();
            // Visar userdetails-fönster
            userDetailsWindow.Show();
        }

        // EVENT att "prenumerera" på för relevanta fönster 
        // När login lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler? LogInSuccess; // Make event nullable

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
