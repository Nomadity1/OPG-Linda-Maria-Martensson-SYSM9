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
        //private readonly UserManager _userManager; // Behövs denna? 
        private readonly RecipeManager _recipeManager;
        private readonly User _currentUser;
        // PUBLIKA EGENSKAPER med effektiv deklaration
        public ObservableCollection<Recipe> Recipes { get; set; }
        public Recipe SelectedRecipe { get; set; }
        public string CurrentUser => _currentUser.UserName;

        // PUBLIK EGENSKAP som använder sig av UserManager- och RecipeManagerklassen (skapar förutsättning för samarbete)
        public UserManager UserManager { get; }
        public RecipeManager RecipeManager { get; }

        // PUBLIKA METOD-DEFINITIONER FÖR KOMMANDON I LAMBDAUTTRYCK (EFFEKTIV FORM) som använder basklass RelayCommand)
        public RelayCommand AddRecipeCommand => new RelayCommand(AddRecipe);
        public RelayCommand RecipeDetailsCommand => new RelayCommand(OpenRecipeDetails);
        public RelayCommand DeleteRecipeCommand => new RelayCommand(DeleteRecipe);
        public RelayCommand LogOutCommand => new RelayCommand(LogOut);
        public RelayCommand UserDetailsCommand => new RelayCommand(OpenUserDetails);
        public RelayCommand InfoCommand => new RelayCommand(ShowInfo);

        // KONSTRUKTOR som upprättar samarbete med RecipeManager // Behövs samarbete med UserManager? 
        public RecipeListViewModel(User user, RecipeManager recipeManager)
        {
            _currentUser = user;
            //_userManager = new UserManager();
            _recipeManager = recipeManager;
            //recipeManager = new RecipeManager();
            if (user is AdminUser)
            {
                // Admin-användare får se alla recept
                Recipes = new ObservableCollection<Recipe>(_recipeManager.GetAllRecipes());
            }
            else
            {
                // "Vanlig" användare får bara se sina egna recept
                Recipes = new ObservableCollection<Recipe>(_recipeManager.GetUserRecipes(user.UserName));
            }
        }
        public RecipeListViewModel()
        { 
        }

        // METODER för knappar för att LÄGGA TILL RECEPT, ÖPPNA RECEPTDETALJER och STÄNGA FÖNSTER
        public void AddRecipe(object parameter)
        {
            // Instansierar AddRecipeViewModel och visar vyn för aktuella objekt
            var addRecipeVM = new AddRecipeViewModel(_recipeManager, _currentUser);
            var addRecipeWindow = new AddRecipeWindow(addRecipeVM);
            addRecipeWindow.Show();
            // Anropar fönsterstängare
            CloseCurrentWindow();
        }
        public void OpenRecipeDetails(object parameter)
        {
            // Har ett recept valts?
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Markera ett recept först!");
                return;
            }
            // Instansierar RecipeDetailViewModel och visar vyn för aktuella objekt
            var recipeDetailVM = new RecipeDetailViewModel(SelectedRecipe, _recipeManager, _currentUser);
            var recipeDetailWindow = new RecipeDetailWindow();
            recipeDetailWindow.Show();
            // Anropar fönsterstängare
            CloseCurrentWindow();
        }
        private void DeleteRecipe(object parameter)
        {
            // Har ett recept valts?
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Markera ett recept först!");
                return;
            }
            // Kollar behörighet genom att kolla roll (
            if (_currentUser is AdminUser || SelectedRecipe.Owner == _currentUser.UserName)
            {
                _recipeManager.RemoveRecipe(SelectedRecipe);
                Recipes.Remove(SelectedRecipe);
            }
            else
            {
                MessageBox.Show("Du kan bara ta bort dina egna recept!");
            }
        }
        private void OpenUserDetails(object parameter)
        {
            // Instansierar UserDetailsViewModel och visar vyn för aktuella objekt
            var userDetailsVM = new UserDetailsViewModel(_currentUser);
            var window = new Views.UserDetailsWindow();
            window.Show();
            CloseCurrentWindow();
        }
        private void LogOut(object parameter)
        {
            // Instansierar MainWindow och visar inloggningsvyn (= main)
            var mainWindow = new MainWindow(); 
            mainWindow.Show();
            // Anropar fönsterstängare
            CloseCurrentWindow();
        }
        private void ShowInfo(object parameter)
        {
            // Visar information om applikationen
            MessageBox.Show("CookMaster – En superavancerad app för dina bästa smakupplevelser.");
        }

        // EN METOD för att stänga fönster ( Hade jag kunnat lägga mera centralt kanske? ) 
        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Views.RecipeListWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
