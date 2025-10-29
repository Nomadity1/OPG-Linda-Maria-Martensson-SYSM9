using CookMaster.Managers;
using CookMaster.MVVM;
using CookMaster.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CookMaster.ViewModels
{
    class UserDetailsViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _updatedUsername;
        private string _updatedPassword;
        private string _repeatedPassword;
        private string _updatedEmail;
        private string _updatedSelectedCountry;
        private string _error;

        // KONSTRUKTOR 
        public UserDetailsViewModel(UserManager userManager)
        {
            // Tilldelar värde/parameter
            _userManager = userManager;
            _updatedUsername = string.Empty; // Initierar med tom string
            _updatedPassword = string.Empty; // Initierar med tom string
            _repeatedPassword = string.Empty; // Initierar med tom string
            _updatedEmail = string.Empty; // Initierar med tom string
            _updatedSelectedCountry = string.Empty; // Initierar med tom string
            _error = string.Empty; // Initierar med tom string
        }

        // ANVÄNDA REGISTER som förlaga! 
        // PUBLIKA EGENSKAPER med mera effektiv deklaration 
        public string UpdatedUserName
        {
            get => _updatedUsername;
            set
            {
                _updatedUsername = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public string UpdatedPassword
        {
            get => _updatedPassword;
            set
            {
                _updatedPassword = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public string RepeatedPassword
        {
            get => _repeatedPassword;
            set
            {
                _repeatedPassword = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public string UpdatedEmail
        {
            get => _updatedEmail;
            set
            {
                _updatedEmail = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // Koppla till listan över länder i UserManager så att den
        // kan fungera som "ItemsSource" i View 
        public List<string> Countries => _userManager.Countries;

        public string UpdatedSelectedCountry
        {
            get => _updatedSelectedCountry;
            set
            {
                _updatedSelectedCountry = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public RelayCommand UpdateCommand => new RelayCommand(execute => Update(), canExecute => CanUpdate());

        // METOD för att aktivera registreringssknapp
        private bool CanUpdate() =>
            !string.IsNullOrWhiteSpace(UpdatedUserName)
            && !string.IsNullOrWhiteSpace(UpdatedEmail)
            && !string.IsNullOrWhiteSpace(UpdatedPassword)
            && !string.IsNullOrWhiteSpace(RepeatedPassword)
            && !string.IsNullOrWhiteSpace(UpdatedSelectedCountry);

        // METOD för uppdateringskommando 
        public void Update()
        {
            // Anropar UpdateDetails-metod i UserManager
            //var (success, message) = _userManager.Register(NewUserName, Email, NewPassword, RepeatPassword, SelectedCountry);
            var success = _userManager.UpdateDetails();
            // Kollar matchning genom att anropa metod i UserManager
            // OM inloggning lyckas anropas (invoke) EVENTET (definierat längst ner i denna fil) LogInSuccess 
            // ...som meddelar (Invoke - en metod) alla "prenumeranter", dvs. alla delar i appen som lyssnar på eventet.
            // ? = "Om OnLoginSuccess inte är null, kalla Invoke(); annars gör inget"
            // this = Referens till den aktuella instansen av klass som gör anropet
            // System.EventArgs.Empty = standardargument när inga specifika data behöver skickas med evenetet 
            if (success)
                UpdateSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = message; // Tar meddelanden från UserManager 
        }

        // EVENT som userdetails-fönstret "prenumererar" på
        // När ändringar lyckas, körs alla metoder som är kopplade till detta event.
        public event System.EventHandler? UpdateSuccess; // Make event nullable

        //        Inputruta för att välja nytt användarnamn.
        //• Om ett användarnamn är upptaget ska ett varningsmeddelande
        //visas när användaren försöker spara.
        //• Om användaren försöker skapa ett användarnamn som är kortare
        //än 3 tecken ska ett varningsmeddelande dyka upp när användaren
        //försöker spara. 
        //• Inputrutor för att välja nytt lösenord. 
        //• Input i de två rutorna "New password" och "Confirm password" 
        //måste överensstämma för att ett lösenord ska kunna ändras, annars
        //dyker ett varningsmeddelande upp när användaren försöker spara.
        //• Om användaren försöker skapa ett lösenord som är kortare än 5 
        //tecken ska ett varningsmeddelande dyka upp när användaren
        //försöker spara. 
        //• ComboBox för att välja ett nytt land.
        //• "Save"-knapp för att spara nya användaruppgifter och stänga
        //UserDetailsWindow.
        //• Cancel"-knapp för att stänga UserDetailsWindow och återgå till 
        //RecipeListWindow.

        public required string DisplayName { get; set; }
        public required string Role { get; set; } // Skulle kunna sätta ett default value (Member) och sedan kunna tilldela andra 
                                                  // ...roller vid specialtillfällen (Super Member, Administrator)



        //ResetPwd


        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
