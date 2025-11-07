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
    // INLOGGNINGSFÖNSTER - VIEWMODEL
    public class MainViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _username;
        private string _password;
        private string _usernamePwdReq; // För fejkad lösenordsåterställning
        private string _answer; // För fejkad lösenordsåterställning
        private string _error;

        // PUBLIKA EGENSKAPER med  effektiv deklaration 
        public string UserName { get => _username; set { _username = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }
        public string UsernamePwdReq { get => _usernamePwdReq; set { _usernamePwdReq = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }
        public string Answer { get => _answer; set { _answer = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); } }
        public string Error { get => _error; set { _error = value; OnPropertyChanged(); } }

        // PUBLIKA METOD-DEFINITIONER FÖR KOMMANDON I LAMBDAUTTRYCK (EFFEKTIV FORM) som använder basklass RelayCommand)
        // FÖR INLOGGNING, REGISTRERING & EVT. LÖSENORDSÅTERSTÄLLNING
        public RelayCommand LogInCommand => new RelayCommand(execute => Login(), canExecute => CanLogin());
        public RelayCommand OpenRegisterCommand => new RelayCommand(OpenRegister);
        public RelayCommand SendQueryCommand => new RelayCommand(execute => SendQuery(), canExecute => CanSendQuery());
        public RelayCommand SubmitReplyCommand => new RelayCommand(execute => SubmitReply(), canExecute => CanSubmitReply());

        // METODER för att aktivera knappar ( i de fall när jag vill att de är inaktiva tills uppgifter fyllts i )
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); // Här kontrolleras att inmatning skett
        private bool CanSendQuery() => !string.IsNullOrWhiteSpace(UsernamePwdReq);
        private bool CanSubmitReply() => !string.IsNullOrWhiteSpace(Answer);

        // KONSTRUKTOR som upprättar samarbete med UserManager 
        public MainViewModel()
        {
            _userManager = (UserManager)Application.Current.Resources["UserManager"];
        }

        // METODER FÖR KOMMANDON - INLOGGNING och ÖPPPNA REGISTRERING
        private void Login()
        {
            // Anropar LogIn-metoden i UserManager och tar emot returvärden
            var (success, message) = _userManager.LogIn(UserName, Password);
            if (success)
                LogInSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = message; // Tar meddelande från userManager
            //Fönster stängs och öppnas i* VM.xaml.cs
        }

        // Metod för att skicka säkerhetsfråga
        public void SendQuery()
        {
            // Kollar att användarnamn är ifyllt
            if (string.IsNullOrWhiteSpace(UsernamePwdReq))
            {
                MessageBox.Show("Användarnamn måste fyllas i.");
                return;
            }
            // Anropar metod i UserManager och tar emot returvärden
            var (success, message) = _userManager.ForgotPassword(UsernamePwdReq);
            if (success)
                MessageBox.Show(message); // Meddelande frpn usermanager
            else
                Error = message; // meddelande från usermanager
        }
        public void SubmitReply()
        {
            // Kollar att svar på säkerhetsfråga är ifyllt
            if (string.IsNullOrWhiteSpace(Answer))
            {
                MessageBox.Show("Svar på säkerhetsfråga måste fyllas i.");
                return;
            }
            // Anropar metod i UserManager och tar emot returvärden
            var (success, message) = _userManager.ValidateReply(Answer);
            if (success)
                MessageBox.Show(message); // meddelande från usermanager
            else
                Error = message; // meddelande från usermanager
        }
        // METOD för kommando att öppna registrering
        public void OpenRegister(object parameter) // Ingen inmatning krävs, tar bara emot objekt-parameter från knapp
        {
            var registerWindow = new RegisterWindow(); // Instansierar registreringsvyn
            registerWindow.ShowDialog(); // Visar registreringsvyn som dialogruta (kan då komma tillbaka till inloggningsvyn efter reg.)
        }

        // EVENT att "prenumerera" på för relevanta fönster 
        public event System.EventHandler? LogInSuccess; // Nullable

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
