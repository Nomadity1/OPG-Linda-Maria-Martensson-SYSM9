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
    // LÄGG TILL RECEPT - VIEWMODEL
    public class AddRecipeViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly RecipeManager _recipeManager;
        private readonly UserManager _userManager;
        private string _title;
        private string _ingredients;
        private string _instructions;
        public string _cookingTime;
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
        public string CookingTime { get => _cookingTime;
            set { _cookingTime = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Category { get => _category;
            set { _category = value; OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); }
        }
        public string Error { get => _error; 
            set { _error = value; OnPropertyChanged(); }
        }

        // KOMMANDO-DEFINITIONER via ICommand in RelayCommandManager med effektiv deklaration
        public RelayCommand SaveCommand => new RelayCommand(execute => SaveRecipe(), canSaveRecipe => CanSaveRecipe());
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        // METOD för att aktivera sparaknapp (cancel alltid aktiv)
        private bool CanSaveRecipe() =>
           !string.IsNullOrWhiteSpace(Title)
           && !string.IsNullOrWhiteSpace(Ingredients)
           && !string.IsNullOrWhiteSpace(Instructions)
           && !string.IsNullOrWhiteSpace(CookingTime)
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
            _cookingTime = string.Empty;
            _category = string.Empty; 
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
        // METODER för kommandon att SPARA och AVBRYTA 
        public void SaveRecipe() 
        {
            // Säkerställer att användaren är inloggad
            if (_userManager.CurrentUser == null)
            {
                MessageBox.Show("Du måste vara inloggad för att lägga till recept!");
                return;
            }
            // Kontrollera om titel redan finns 
            if (Title == null)
            {
                MessageBox.Show("Titeln är upptagen.");
                return;
            }
            // KOllar att alla fält är ifyllda
            if (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Ingredients)
                && !string.IsNullOrWhiteSpace(Instructions) && !string.IsNullOrWhiteSpace(CookingTime)
                && !string.IsNullOrWhiteSpace(Category))
            {
                MessageBox.Show("Alla fält måste vara ifyllda.");
                return;
            }
            // Anropar metod i RecipeManager med owner från UserManager
            var owner = _userManager.CurrentUser.UserName;
            var (success, message) = _recipeManager.AddRecipe(Title, Ingredients, Instructions, CookingTime, Category, owner);
            if (!success) 
            {
                SaveSuccess?.Invoke(this, System.EventArgs.Empty);
                Error = message; // Tar meddelande från recipeManager
            }
            // ANNARS sparas receptet och användaren meddelas
            MessageBox.Show("Recept sparat!");
            // Anropar fönsterstängare
            WindowCloser();
        }
        private void Cancel(object parameter) // Object parameter för RelayCommand 
        {
            // Anropar fönsterstängare
            WindowCloser();
        }

        // EVENT att "prenumerera" på för relevanta fönster 
        public event System.EventHandler? SaveSuccess; // nullable

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}