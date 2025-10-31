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
        private string _updatedRepeatedPassword;
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
            _updatedRepeatedPassword = string.Empty; // Initierar med tom string
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
        public string UpdatedRepeatedPassword
        {
            get => _updatedRepeatedPassword;
            set
            {
                _updatedRepeatedPassword = value; OnPropertyChanged();
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
        public RelayCommand UpdateUserNameCommand => new RelayCommand(execute => UpdateUserName(), canExecute => CanUpdateUserName());

        // METOD för att aktivera registreringssknapp
        private bool CanUpdateUserName() =>
            !string.IsNullOrWhiteSpace(UpdatedUserName);

        // METODER för att ändra användaruppgifter - Kopplat till uppdateringskommando 
        public void UpdateUserName()
        {
            // Grundantagande: updated user details != user details 
            // Anropar UpdateDetails-metod i UserManager
            //UpdatedUserName, UpdatedEmail, UpdatedPassword, RepeatedPassword, UpdatedSelectedCountry
            //var (success, message) = _userManager.Register(NewUserName, Email, NewPassword, RepeatPassword, SelectedCountry);
            var (success, message) = _userManager.UserNameUpdate(UpdatedUserName);
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
        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public RelayCommand UpdatePasswordCommand => new RelayCommand(execute => UpdatePassword(), canExecute => CanUpdatePassword());

        // METOD för att aktivera registreringssknapp
        private bool CanUpdatePassword() =>
            !string.IsNullOrWhiteSpace(UpdatedPassword)
            && !string.IsNullOrWhiteSpace(UpdatedRepeatedPassword);

        // METODER för att ändra användaruppgifter - Kopplat till uppdateringskommando 
        public void UpdatePassword()
        {
            // Grundantagande: updated user details != user details 
            // Anropar UpdateDetails-metod i UserManager
            //UpdatedUserName, UpdatedEmail, UpdatedPassword, RepeatedPassword, UpdatedSelectedCountry
            //var (success, message) = _userManager.Register(NewUserName, Email, NewPassword, RepeatPassword, SelectedCountry);
            var (success, message) = _userManager.UpdateDetails(UpdatedPassword, UpdatedRepeatedPassword);
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
        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public RelayCommand UpdateEmailCommand => new RelayCommand(execute => UpdateEmail(), canExecute => CanUpdateEmail());

        // METOD för att aktivera registreringssknapp
        private bool CanUpdateEmail() =>
            !string.IsNullOrWhiteSpace(UpdatedEmail);

        // METODER för att ändra användaruppgifter - Kopplat till uppdateringskommando 
        public void UpdateEmail()
        {
            // Grundantagande: updated user details != user details 
            // Anropar UpdateDetails-metod i UserManager
            //UpdatedUserName, UpdatedEmail, UpdatedPassword, RepeatedPassword, UpdatedSelectedCountry
            //var (success, message) = _userManager.Register(NewUserName, Email, NewPassword, RepeatPassword, SelectedCountry);
            var (success, message) = _userManager.UpdateDetails(UpdatedEmail);
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

        // REGISTER-KOMMANDO via ICommand in RelayCommandManager
        public RelayCommand UpdateCountryCommand => new RelayCommand(execute => UpdateCountry(), canExecute => CanUpdateCountry());

        // METOD för att aktivera registreringssknapp
        private bool CanUpdateCountry() =>
            !string.IsNullOrWhiteSpace(UpdatedSelectedCountry);

        // METODER för att ändra användaruppgifter - Kopplat till uppdateringskommando 
        public void UpdateCountry()
        {
            // Grundantagande: updated user details != user details 
            // Anropar UpdateDetails-metod i UserManager
            //UpdatedUserName, UpdatedEmail, UpdatedPassword, RepeatedPassword, UpdatedSelectedCountry
            //var (success, message) = _userManager.Register(NewUserName, Email, NewPassword, RepeatPassword, SelectedCountry);
            var (success, message) = _userManager.UpdateDetails(UpdatedSelectedCountry);
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

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
