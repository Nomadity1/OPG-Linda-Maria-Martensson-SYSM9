using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CookMaster.ViewModels
{
    // ANVÄNDARUPPGIFTER - VIEWMODEL
    public class UserDetailsViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _updatedUsername;
        private string _updatedPassword;
        private string _updatedRepeatedPassword;
        private string _updatedSelectedCountry;
        private string _error;
        private User currentUser;

        // PUBLIKA EGENSKAPER med mera effektiv deklaration 
        public string UpdatedUserName
        { get => _updatedUsername;
            set { _updatedUsername = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string UpdatedPassword
        { get => _updatedPassword;
            set { _updatedPassword = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string UpdatedRepeatedPassword
        { get => _updatedRepeatedPassword;
            set { _updatedRepeatedPassword = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public List<string> Countries => _userManager.Countries;

        public string UpdatedSelectedCountry
        { get => _updatedSelectedCountry;
            set { _updatedSelectedCountry = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error
        { get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        // PUBLIKA KOMMANDON för att ändra uppgifter via ICommand i RelayCommandManager
        public RelayCommand SaveUpdatesCommand =>
            new RelayCommand(execute => SaveUpdates(), canExecute => CanSaveUpdates());
        public RelayCommand CancelCommand => new RelayCommand(CancelUserDetails);

        // METOD för att aktivera knapp
        private bool CanSaveUpdates() =>
            !string.IsNullOrWhiteSpace(UpdatedUserName);

        // KONSTRUKTOR 
        public UserDetailsViewModel(UserManager userManager)
        {
            // Upprätta samarbete med UserManager
            // Kasta undantag om userManager är null
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            // Hämta aktuell användare och initiera visade fält
            // Om ingen aktuell användare finns, kasta undantag
            currentUser = _userManager.CurrentUser ?? throw new InvalidOperationException("No current user available.");
            
            // Tilldelar värden att visa i textboxar
            _updatedUsername = currentUser.UserName;
            _updatedSelectedCountry = currentUser.Country;

            // Lämnar vissa boxar tomma för hägre säkerhet
            _updatedPassword = string.Empty;
            _updatedRepeatedPassword = string.Empty;
            _error = string.Empty;
        }

        // FÖNSTERSTÄNGARE 
        private void WindowCloser()
        {
            foreach (Window w in Application.Current.Windows.OfType<Window>().ToList())
            {
                if (!(w is RecipeListWindow))
                    w.Close();
            }
        }

        // METODER för att ändra användaruppgifter
        // SPARA Kopplat till spara-uppdateringar-kommando 
        public void SaveUpdates()
        {
            // Lokala dummy-variabler för lösenord om det inte ändras (är ju tomma i user details)
            var passwordToUse = string.IsNullOrEmpty(UpdatedPassword) ? currentUser.Password : UpdatedPassword;
            var repeatToUse = string.IsNullOrEmpty(UpdatedRepeatedPassword) ? currentUser.Password : UpdatedRepeatedPassword;
            if (passwordToUse != repeatToUse) // använder lokala variabler istället
            {
                Error = "Lösenorden matchar inte!";
                return;
            }
            // Anropar metod i UserManager
            var (success, message) = _userManager.Update(UpdatedUserName, UpdatedPassword, UpdatedRepeatedPassword, UpdatedSelectedCountry);
            // OM lyckas anropas EVENTET UpdateSuccess som meddelar prenumeranter
            if (success)
            {
                currentUser = _userManager.CurrentUser!; // Uppdaterar currentUser med ny info
                UpdateSuccess?.Invoke(this, System.EventArgs.Empty);
            }
            else
                Error = message; // Tar meddelanden från UserManager
            // Fönster stängs i *VM.xaml.cs
        }

        // AVBRYT
        public void CancelUserDetails(object parameter) // Ingen inmatning krävs, tar bara emot objekt-parameter från knapp
        {
            // Anropar fönsterstängare
            WindowCloser();
        }
        // EVENT som userdetails-fönstret "prenumererar" på
        public event System.EventHandler? UpdateSuccess; // Make event nullable

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}