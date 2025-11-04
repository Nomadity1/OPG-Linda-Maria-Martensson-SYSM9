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
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CookMaster.ViewModels
{
    public class AddRecipeViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly RecipeManager _recipeManager;
        private readonly UserManager _userManager;
        private string _title;
        private string _ingredients;
        private string _instructions;
        private string _category;
        private string _error;

        // PUBLIKA EGENSKAPER med mera effektiv deklaration 
        public string Title { get => _title;
            set { _title = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Ingredients { get => _ingredients;
            set { _ingredients = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Instructions { get => _instructions;
            set { _instructions = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Category { get => _category;
            set { _category = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error { get => _error; 
            set { _error = value; OnPropertyChanged(); }
        }

        // Deklerar och initierar KOMMANDOM via ICommand in RelayCommandManager med effektiv deklaration
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveRecipe(), canSaveRecipe => CanSaveRecipe());
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        // METOD för att aktivera sparaknapp (cancel alltid aktiv)
        private bool CanSaveRecipe() =>
           !string.IsNullOrWhiteSpace(Title)
           && !string.IsNullOrWhiteSpace(Ingredients)
           && !string.IsNullOrWhiteSpace(Instructions)
           && !string.IsNullOrWhiteSpace(Category);

        // KONSTRUKTOR som upprättar samarbete med RecipeManager och User
        public AddRecipeViewModel()
        {
            // Tilldelar värden för samarbete m globala variabler
            _userManager = (UserManager)Application.Current.Resources["UserManager"];
            _recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];
            // Tilldelar värde/parameter
            _title = string.Empty;
            _ingredients = string.Empty;
            _instructions = string.Empty;
            _category = string.Empty; 
        }
        // FÖNSTERSTÄNGARE
        private void CloseWindow<T>() where T : Window
        {
            var win = Application.Current.Windows.OfType<T>().FirstOrDefault();
            win?.Close();
        }
        // METODER för kommandon att SPARA, AVBRYTA och STÄNGA FÖNSTER
        public void SaveRecipe() 
        {
            // Säkerställer att användaren är inloggad
            if (_userManager.CurrentUser == null)
            {
                MessageBox.Show("Du måste vara inloggad för att lägga till recept!");
                return;
            }
            // Anropar metod i RecipeManager med owner från UserManager
            var owner = _userManager.CurrentUser.UserName;
            var (success, message) = _recipeManager.AddRecipe(Title, Ingredients, Instructions, Category, owner);
            if (!success) 
            {
                Error = message; // Tar meddelande från recipeManager
            }
            // ANNARS sparas receptet och användaren meddelas
            MessageBox.Show("Recept sparat!");
            // Anropar fönsterstängare
            CloseWindow<AddRecipeWindow>();
            // Öppnar receptlistan igen
            //var recipeListVM = new RecipeListViewModel();
            var recipeListWindow = new RecipeListWindow();
            recipeListWindow.Show();
            // BEHÖVS DET EN REFRESH LIST HÄR?
            // RefreshRecipes();
        }
        private void Cancel(object parameter) // Object parameter för RelayCommand 
        {
            // Anropar fönsterstängare
            CloseWindow<AddRecipeWindow>();
            // Öppnar receptlistan igen
            //var recipeListVW = new RecipeListViewModel();
            var recipeListWindow = new RecipeListWindow();
            recipeListWindow.Show();
        }
        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
