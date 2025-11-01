using CookMaster.Managers;
using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookMaster.ViewModels
{
    public class AddRecipeViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly RecipeManager _recipeManager;
        private readonly User _user;
        // PUBLIKA EGENSKAPER med effektiv deklaration
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Category { get; set; }
        // Deklaration av KOMMANDOM via ICommand in RelayCommandManager med effektiv deklaration
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }
        
        // INGA METODER för att aktivera knappar- Alltid aktiva 

        // KONSTRUKTOR som upprättar samarbete med RecipeManager och User
        public AddRecipeViewModel(RecipeManager recipeManager, User user)
        {
            // Tilldelar värden 
            _recipeManager = recipeManager;
            _user = user;
            // Initierar kommandon
            SaveCommand = new RelayCommand(SaveRecipe);
            CancelCommand = new RelayCommand(Cancel);
        }

        // METODER för knappar för att SPARA, AVBRYTA och STÄNGA FÖNSTER
        public void SaveRecipe(object parameter) 
        {
            // Alla fält ifyllda? 
            if (string.IsNullOrWhiteSpace(Title) ||
                string.IsNullOrWhiteSpace(Ingredients) ||
                string.IsNullOrWhiteSpace(Instructions) ||
                string.IsNullOrWhiteSpace(Category))
            {
                // Annars: 
                MessageBox.Show("Alla fält måste vara ifyllda!");
                return;
            }
            // Skapar recept och lägger till det i RecipeManager, samt meddelar användaren
            var recipe = new Recipe(Title, Ingredients, Instructions, Category, _user.UserName);
            _recipeManager.AddRecipe(recipe);
            MessageBox.Show("Recept sparat!");
            // Instansierar RecipeListViewModel med aktuella objekt (användaren och recepthanterare)
            var listVM = new RecipeListViewModel(_user, _recipeManager);
            // Instansierar och visar listfönstret
            var listWindow = new Views.RecipeListWindow(listVM);
            listWindow.Show();
            // Anropar fönsterstängare
            CloseCurrentWindow();
        }
        private void Cancel(object parameter) // Object parameter för RelayCommand 
        {
            // Instansierar recept VM och VIEW - med aktuell användaren och recepthanterare 
            var recipelistVM = new RecipeListViewModel(_user, _recipeManager); // måste jag involvera denna med anrop? 
            var recipelistWindow = new Views.RecipeListWindow(recipelistVM);
            recipelistWindow.Show();
            // Anropar fönsterstängare
            CloseCurrentWindow();
        }
        // EN METOD för att stänga fönster
        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Views.AddRecipeWindow)
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
