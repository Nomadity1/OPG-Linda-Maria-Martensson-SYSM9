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
        private string _error;

        // PUBLIKA EGENSKAPER med  effektiv deklaration 
        public string UserName
        { get => _username; set { _username = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Password
        { get => _password; set { _password = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error
        { get => _error; set { _error = value; OnPropertyChanged(); }
        }

        // KONSTRUKTOR som upprättar samarbete med UserManager resp RecipeManager
        public MainViewModel() 
        {
            _userManager = (UserManager)Application.Current.Resources["UserManager"];
        }

        // PUBLIKA METOD-DEFINITIONER FÖR KOMMANDON I LAMBDAUTTRYCK (EFFEKTIV FORM) som använder basklass RelayCommand)
        // FÖR INLOGGNING, REGISTRERING & EVT. LÖSENORDSÅTERSTÄLLNING
        public RelayCommand LogInCommand => new RelayCommand(execute => Login(), canExecute => CanLogin());
        public RelayCommand OpenRegisterCommand => new RelayCommand(OpenRegister);

        // METODER för att aktivera knappar ( i de fall när jag vill att de är inaktiva tills uppgifter fyllts i )
        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); // Här kontrolleras att inmatning skett

        // METOD - INLOGGNING 
        private void Login()
        {
            // Anropar LogIn-metoden i UserManager och tar emot returvärden
            var (success, message) = _userManager.LogIn(UserName, Password);
            if (success)
                LogInSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = message; // Tar meddelande från userManager
        }

        // METOD för kommando att öppna registrering
        public void OpenRegister(object parameter) // Ingen inmatning krävs, tar bara emot objekt-parameter från knapp
        {
            // Öppnar registreringsvyn
            RegisterViewModel registerVM = new RegisterViewModel();
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
            // Anropar fönsterstängare
            //CloseCurrentWindow();
        }

        // EVENT att "prenumerera" på för relevanta fönster 
        // När begäran lyckas, körs alla metoder som är kopplade till eventet
        public event System.EventHandler? LogInSuccess; // Make event nullable

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
