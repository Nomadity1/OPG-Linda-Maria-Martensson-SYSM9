using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class RecipeDetailViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly Recipe _recipe;
        private readonly RecipeManager _recipeManager;
        private readonly UserManager _userManager;
        private bool _isReadOnly = true; // styr möjlighet att redigera 

        // PUBLIKA EGENSKAPER med effektiv deklaration
        public string Title { get => _recipe.Title; set { _recipe.Title = value; OnPropertyChanged(); } }
        public string Ingredients { get => _recipe.Ingredients; set { _recipe.Ingredients = value; OnPropertyChanged(); } }
        public string Instructions { get => _recipe.Instructions; set { _recipe.Instructions = value; OnPropertyChanged(); } }
        public string Category { get => _recipe.Category; set { _recipe.Category = value; OnPropertyChanged(); } }
        public DateTime Date { get => _recipe.Date; }
        public string CurrentRecipe => _recipeManager.CurrentRecipe?.Title ?? string.Empty; // Visa aktuellt recept
        public ObservableCollection<Recipe> Recipes { get; set; } // Dynamisk lista för recephantering 
        
        // PUBLIK BOOL som styr textboxarnas redigerbarhet
        public bool IsReadOnly { get => _isReadOnly; set { _isReadOnly = value; 
                OnPropertyChanged();
                // Tvingar kommandot att stämma av med CanExecute när Edit ändras
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // PUBLIKA KOMMANDODEFINOITIONER (via ICommand in RelayCommandManager)
        public RelayCommand EditCommand => new RelayCommand(Edit, _ => _userManager?.CurrentUser != null);
        // Kan bara redigera om användare är inloggad
        public RelayCommand SaveCommand => new RelayCommand(execute => Save(), canExecute => CanSave());
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        // BOOL för att aktivera SPARA-KNAPP
        public bool CanSave() =>
            !string.IsNullOrWhiteSpace(Title)
            && !string.IsNullOrWhiteSpace(Ingredients)
            && !string.IsNullOrWhiteSpace(Instructions)
            && !string.IsNullOrWhiteSpace(Category);

        // KONSTRUKTO 
        public RecipeDetailViewModel(Recipe recipe)
        {
            // Globala managers
            _userManager = (UserManager)Application.Current.Resources["UserManager"];
            _recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];

            // Tilldelar receptet
            _recipe = recipe ?? throw new ArgumentNullException(nameof(recipe));
            // Prenumererar på eventet i RecipeManager för att uppdatera CurrentRecipe när aktuellt recept ändras
            _recipeManager.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(RecipeManager.CurrentRecipe))
                {
                    OnPropertyChanged(nameof(CurrentRecipe));
                }
            };
            // Initierar den dynamiska listan
            Recipes = new ObservableCollection<Recipe>(_recipeManager.GetAllRecipes());
        }
        // Parameterlös konstruktor som hämta aktuellt recept från RecipeManager - TIPS FRÅN GITHUB COPILOT
        public RecipeDetailViewModel() : this((Application.Current.Resources["RecipeManager"] as RecipeManager)?.CurrentRecipe
                                               ?? throw new InvalidOperationException("No recipe provided to RecipeDetailViewModel"))
        { }

        // FÖNSTERSTÄNGARE
        private void WindowCloser()
        {
            foreach (Window w in Application.Current.Windows.OfType<Window>().ToList())
            {
                if (!(w is RecipeListWindow))
                    w.Close();
            }
        }

        // METODER för KOMMANDON - REDIGERA, SPARA & AVBRYT
        private void Edit(object parameter)
        {
            if (_userManager.CurrentUser == null)
            {
                MessageBox.Show("Du måste vara inloggad för att redigera ett recept!");
                return;
            }
            if (_recipe == null)
            {
                MessageBox.Show("Du måste välja ett recept för att kunna redigera.");
                return;
            }
            if (_userManager.CurrentUser.UserName != _recipe.Owner && !(_userManager.CurrentUser is AdminUser))
            {
                MessageBox.Show("Du kan bara redigera dina egna recept.");
                return;
            }
            else
                // Öppnar för redigering
                IsReadOnly = false;
            // Fönster ska inte stängas
        }
        private void Save()
        {
            // Kontrollera att recept finns
            if (_recipe == null)
            {
                MessageBox.Show("Inget recept valt för att spara.");
                return;
            }
            // Kontrollera att användare är inloggad
            var currentUser = _userManager.CurrentUser;
            if (currentUser == null) {
                MessageBox.Show("Du måste vara inloggad för att spara ett recept!");
                return;
            }
            // Kontrollera ägarskap eller adminrättigheter
            if (currentUser.UserName != _recipe.Owner && !(currentUser is AdminUser))
            {
                MessageBox.Show("Du kan bara spara ändringar på dina egna recept.");
                return;
            }
            // ANNARS Anropas metoden i RecipeManager 
            var (success, message) = _recipeManager.UpdateRecipe(_recipe);
            if (success)
                IsReadOnly = true; // Återgår till read-only 
            SaveSuccess?.Invoke(this, System.EventArgs.Empty);
            // Fönster stängs i *VM.xaml.cs
        }
        private void Cancel(object parameter)
        {
            // Återställer ev ändringar 
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Ingredients));
            OnPropertyChanged(nameof(Instructions));
            OnPropertyChanged(nameof(Category));
            IsReadOnly = true; // Återgår till read-only
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