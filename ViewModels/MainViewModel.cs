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
        private string _usernamePwdReq; // Används för lösenordsåterställning
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
        public string UsernamePwdReq
        {
            get => _usernamePwdReq;
            set { _usernamePwdReq = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
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

        // KONSTRUKTOR som upprättar samarbete mln Main och UserManager resp RecipeManager
        public MainViewModel() 
        {
            _userManager = new UserManager();
            //_recipeManager = new RecipeManager(); SENARE!
        }

        // PUBLIKA METOD-DEFINITIONER FÖR KOMMANDON I LAMBDAUTTRYCK (EFFEKTIV FORM) som använder basklass RelayCommand)
        // FÖR INLOGGNING, REGISTRERING & LÖSENORDSÅTERSTÄLLNING
        public RelayCommand LogInCommand => new RelayCommand(execute => Login(), canExecute => CanLogin());
        public RelayCommand OpenRegisterCommand => new RelayCommand(execute => OpenRegister(), canExecute => CanOpenRegister());
        public RelayCommand ForgotPasswordCommand => new RelayCommand(execute => ForgotPassword(), canExecute => CanForgotPassword());
        public RelayCommand ResetPasswordCommand => new RelayCommand(execute => ResetPassword(), canExecute => CanResetPassword());

        // METODER för att aktivera knappar
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        private bool CanOpenRegister() => true; // Alltid aktiv
        private bool CanForgotPassword() =>
            !string.IsNullOrWhiteSpace(UsernamePwdReq);
        private bool CanResetPassword() =>
            !string.IsNullOrWhiteSpace(Answer);

        // METOD - INLOGGNING 
        private void Login()
        {
            // Kontrollera att alla fält är ifyllda 
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Alla fält måste fyllas i.");
                return;
            }
            // Anropar metod i UserManager, skickar input dit för att validera begäran
            else if (_userManager.ValidateLogin(UserName, Password))
            {
                LogInSuccess?.Invoke(this, System.EventArgs.Empty);
            }
            // Om inloggning lyckas anropas (invoke) EVENTET LogInSuccess, som alla "prenumeranter" lyssnar på
            // ? = OM success inte är null, kalla Invoke(); - annars gör inget 
            // this = Referens till den aktuella instansen av klass som gör anropet
            // System.EventArgs.Empty = standardargument när inga specifika data behöver skickas med eventet 
            else
                Error = "Fel användarnamn eller lösenord";
        }

        // METOD för kommando att öppna registrering
        public void OpenRegister()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
        public void ForgotPassword() // UsernamePwdReq, Answer || !string.IsNullOrWhiteSpace(Password)
        {
            // Kontrollera att  fält är ifyllt
            if (string.IsNullOrWhiteSpace(UsernamePwdReq))
            {
                MessageBox.Show("Användarnamn måste fyllas i.");
                return;
            }
        }
        public void ResetPassword() 
        {
            // Anropar metod i UserManager, skickar input dit för att validera begäran
            if (_userManager.ResetPassword(UserName, Answer))
            {
                ResetPasswordSuccess?.Invoke(this, System.EventArgs.Empty);
            }
        }

        // EVENT att "prenumerera" på för relevanta fönster 
        // När begäran lyckas, körs alla metoder som är kopplade till eventet
        public event System.EventHandler? LogInSuccess; // Make event nullable
        public event System.EventHandler? ForgotPasswordSuccess; // Make event nullable
        public event System.EventHandler? ResetPasswordSuccess; // Make event nullable


        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
