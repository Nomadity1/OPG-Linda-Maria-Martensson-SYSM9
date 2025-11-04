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

        // KONSTRUKTOR 
        public UserDetailsViewModel(UserManager userManager)
        {
            // Tilldelar värde/parameter
            _userManager = userManager;
            _updatedUsername = UpdatedUserName; 
            _updatedPassword = UpdatedPassword;
            _updatedRepeatedPassword = UpdatedRepeatedPassword;
            _updatedSelectedCountry = UpdatedSelectedCountry; 
            _error = Error; 
        }
        // KONSTRUKTOR som OVERLOADAR med aktuell användare
        public UserDetailsViewModel(User currentUser)
        {
            this.currentUser = currentUser;
        }
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

        // KOMMANDO för att ändra uppgifter via ICommand in RelayCommandManager
        public RelayCommand SaveUpdatesCommand => 
            new RelayCommand(execute => SaveUpdates(), canExecute => CanSaveUpdates());

        // METOD för att aktivera knapp
        private bool CanSaveUpdates() =>
            !string.IsNullOrWhiteSpace(UpdatedUserName) 
            && !string.IsNullOrWhiteSpace(UpdatedPassword)
            && !string.IsNullOrWhiteSpace(UpdatedRepeatedPassword);

        // METOD som hjälper till att stänga fönster om typ anges i respektive metod 
        private void CloseWindow<T>() where T : Window
        {
            var win = Application.Current.Windows.OfType<T>().FirstOrDefault();
            win?.Close();
        }

        // METODER för att ändra användaruppgifter - Kopplat till uppdateringskommando 
        public void SaveUpdates()
        {
            if (UpdatedPassword != UpdatedRepeatedPassword)
            {
                Error = "Lösenorden matchar inte!";
                return;
            }
            // Anropar metod i UserManager
            var (success, message) = _userManager.Update(UpdatedUserName, UpdatedPassword, UpdatedRepeatedPassword, UpdatedSelectedCountry);
            // OM lyckas anropas EVENTET UpdateSuccess som meddelar prenumeranter
            if (success)
                UpdateSuccess?.Invoke(this, System.EventArgs.Empty);
            else
                Error = message; // Tar meddelanden från UserManager
            //// Anropar fönsterstängare
            //CloseWindow<UserDetailsWindow>();
            //// Öppnar och visar receptvyn
            //var recipeList = new RecipeListWindow();
            //recipeList.ShowDialog();
        }
        public RelayCommand CancelCommand => new RelayCommand(CancelUserDetails);
        public void CancelUserDetails(object parameter) // Ingen inmatning krävs, tar bara emot objekt-parameter från knapp
        {
            // Anropar fönsterstängare
            CloseWindow<UserDetailsWindow>();
            // Instansierar och visar receptvyn
            var recipelistWindow = new RecipeListWindow();
            recipelistWindow.Show();
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