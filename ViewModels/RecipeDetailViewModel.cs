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

namespace CookMaster.ViewModels
{
    class RecipeDetailViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly Recipe _recipe;
        private readonly RecipeManager _recipeManager;
        private readonly User _user;
        // PUBLIKA EGENSKAPER med effektiv deklaration
        public string Title { get => _recipe.Title; set { _recipe.Title = value; OnPropertyChanged(); } }
        public string Ingredients { get => _recipe.Ingredients; set { _recipe.Ingredients = value; OnPropertyChanged(); } }
        public string Instructions { get => _recipe.Instructions; set { _recipe.Instructions = value; OnPropertyChanged(); } }
        public string Category { get => _recipe.Category; set { _recipe.Category = value; OnPropertyChanged(); } }
        private bool _isReadOnly = true;
        // BOOL som styr textboxarnas redigerbarhet
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value; OnPropertyChanged(); // Lägga till commandmanager? 
            }
        }

        // Deklarera KOMMANDON via ICommand in RelayCommandManager
        public RelayCommand EditCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        // KONSTRUKTOR som upprättar samarbete med RecipeManager och User
        public RecipeDetailViewModel(Recipe recipe, RecipeManager recipeManager, User user)
        {
            _recipe = recipe;
            _recipeManager = recipeManager;
            _user = user;
            EditCommand = new RelayCommand(Edit);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        // KONSTRUKTOR utan parametrar 
        public RecipeDetailViewModel()
        {
        }

        // METODER för KOMMANDON
        private void Edit(object parameter)
        {
            if (_user.UserName != _recipe.Owner && !(_user is AdminUser))
            {
                MessageBox.Show("Du kan bara redigera dina egna recept.");
                return;
            }
            IsReadOnly = false;
        }
        private void Save(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Title) ||
                string.IsNullOrWhiteSpace(Ingredients) ||
                string.IsNullOrWhiteSpace(Instructions) ||
                string.IsNullOrWhiteSpace(Category))
            {
                MessageBox.Show("Alla fält måste vara ifyllda!");
                return;
            }
            MessageBox.Show("Receptet är sparat!");
            // Instansierar och visar receptvyn
            var recipeListVM = new RecipeListViewModel(_user, _recipeManager);
            var recipeListWindow = new RecipeListWindow(recipeListVM);
            recipeListWindow.Show();
            // Anropar fönsterstängare
            CloseCurrentWindow(); 
        }
        private void Cancel(object parameter)
        {
            // Instansierar och visar receptvyn
            var recipeListVM = new RecipeListViewModel(_user, _recipeManager);
            var recipeListWindow = new RecipeListWindow(recipeListVM);
            recipeListWindow.Show();
            // Anropar fönsterstängare
            CloseCurrentWindow();
        }
        // EN METOD för att stänga fönster
        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is RecipeDetailWindow)
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
