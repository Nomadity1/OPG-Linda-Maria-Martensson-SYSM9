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

namespace CookMaster.ViewModels
{
    public class RecipeListViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly RecipeManager _recipeManager; // Behövs för att kunna hantera recept
        private readonly UserManager _userManager; // Behövs för att kunna hantera användare
                                             
        // PUBLIKA EGENSKAPER med effektiv deklaration
        public ObservableCollection<Recipe> Recipes { get; set; } // Dynamisk lista för recephantering 
        public Recipe? SelectedRecipe { get; set; } // För att kunna hålla koll på valt recept i listan
        public string CurrentUser => _userManager.CurrentUser?.UserName ?? string.Empty; // Visa inloggad användare 

        // PUBLIKA METOD-DEFINITIONER FÖR KOMMANDON I LAMBDAUTTRYCK (EFFEKTIV FORM) som använder basklass RelayCommand)
        public RelayCommand LogOutCommand => new RelayCommand(LogOut);
        public RelayCommand UserDetailsCommand => new RelayCommand(OpenUserDetails);
        public RelayCommand AddRecipeCommand => new RelayCommand(AddRecipe);
        public RelayCommand RecipeDetailsCommand => new RelayCommand(OpenRecipeDetails);
        public RelayCommand DeleteRecipeCommand => new RelayCommand(DeleteRecipe);
        public RelayCommand InfoCommand => new RelayCommand(ShowInfo);

        // KONSTRUKTOR som upprättar samarbete med RecipeManager och UserManager 
        public RecipeListViewModel()
        {
            // Globala managers 
            _userManager = (UserManager)Application.Current.Resources["UserManager"];
            _recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];
            // Prenumererar på eventet i UserManager för att uppdatera CurrentUser när inloggad användare ändras
            _userManager.PropertyChanged += (s, e) =>             
            {
                if (e.PropertyName == nameof(UserManager.CurrentUser))
                {
                    OnPropertyChanged(nameof(CurrentUser));
                    // Uppdaterar receptlistan när inloggad användare ändras
                    RefreshRecipes();
                }
            };
            // Initial laddning av receptlistan
            RefreshRecipes();
        }
        // METOD som hjälper till att stänga fönster om typ anges i respektive metod 
        private void CloseWindow<T>() where T : Window
        {
            var win = Application.Current.Windows.OfType<T>().FirstOrDefault();
            win?.Close();
        }
        // METOD för att uppdatera receptlistan baserat på inloggad användare
        private void RefreshRecipes()
        {
            var user = _userManager.CurrentUser;
            if (user is AdminUser) // Admin har tillgång till att ändra alla recept
                Recipes = new ObservableCollection<Recipe>(_recipeManager.GetAllRecipes());
            else if (user != null) // Alla andra användare har tillgång till att ändra sina recept
                Recipes = new ObservableCollection<Recipe>(_recipeManager.GetUserRecipes(user.UserName));
            else
                Recipes = new ObservableCollection<Recipe>(); // Alla kan se alla recept
            OnPropertyChanged(nameof(Recipes));
        }
        // METODER för KOMMANDON för att LÄGGA TILL RECEPT, ÖPPNA RECEPTDETALJER och STÄNGA FÖNSTER
        private void LogOut(object parameter)
        {
            _userManager.CurrentUser = null;
            // Anropar fönsterstängare
            CloseWindow<RecipeListWindow>();
            // Öppnar inloggningsfönstret igen
            var mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }
        private void OpenUserDetails(object parameter)
        {
            if (_userManager.CurrentUser == null)
            {
                MessageBox.Show("Du måste vara inloggad för att se användardetaljer!");
                return;
            }
            else
            {
                // Anropar fönsterstängare
                CloseWindow<RecipeListWindow>();
                // Instansierar UserDetailsViewModel och visar vyn för aktuella objekt
                //var userDetailsVM = new UserDetailsViewModel(_userManager.CurrentUser);
                var userDetailsWindow = new UserDetailsWindow();
                //userDetailsWindow.DataContext = userDetailsVM;
                userDetailsWindow.ShowDialog();
            }
        }
        // Öppna add recipe-vyn 
        public void AddRecipe(object parameter)
        {
            if (_userManager.CurrentUser == null)
            {
                MessageBox.Show("Du måste vara inloggad för att lägga till ett recept!");
                return;
            }
            else
            {
                // Anropar fönsterstängare
                CloseWindow<RecipeListWindow>();
                //var addRecipeVM = new AddRecipeViewModel();
                var addRecipeWindow = new AddRecipeWindow();
                addRecipeWindow.ShowDialog();
                // Efter att fönstret stängts, uppdatera receptlistan
            }
            RefreshRecipes();
        }
        public void OpenRecipeDetails(object parameter)
        {
            if (_userManager.CurrentUser == null)
            {
                MessageBox.Show("Du måste vara inloggad för att se receptdetaljer!");
                return;
            }            // Har ett recept valts?
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Markera ett recept först!");
                return;
            }
            // Anropar fönsterstängare
            CloseWindow<RecipeListWindow>();
            // Instansierar RecipeDetailViewModel och visar vyn för aktuella objekt
            //var recipeDetailVM = new RecipeDetailViewModel();
            var recipeDetailWindow = new RecipeDetailWindow();
            //recipeDetailWindow.DataContext = recipeDetailVM;
            recipeDetailWindow.ShowDialog();
            // Anropar metod för att uppdatera receplistan
            RefreshRecipes();
        }
        private void DeleteRecipe(object parameter)
        {
            if (_userManager.CurrentUser == null)
            {
                MessageBox.Show("Du måste vara inloggad för att ta bort ett recept!");
                return;
            }
            // Har ett recept valts?
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Markera ett recept först!");
                return;
            }
            // Kollar behörighet genom att kolla roll 
            if (_userManager.CurrentUser is AdminUser || SelectedRecipe.Owner == _userManager.CurrentUser.UserName)
            {
                _recipeManager.RemoveRecipe(SelectedRecipe);
                Recipes.Remove(SelectedRecipe);
            }
            else
            {
                MessageBox.Show("Du kan bara ta bort dina egna recept!");
            }
            // Anropar metod för att uppdatera receplistan
            RefreshRecipes();
        }
        private void ShowInfo(object parameter)
        {
            // Visar information om applikationen
            MessageBox.Show("CookMaster – En superavancerad app för dina bästa smakupplevelser.");
        }
        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}