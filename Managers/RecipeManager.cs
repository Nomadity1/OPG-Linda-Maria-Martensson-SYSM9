using CookMaster.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Managers
{
    public class RecipeManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {

        private List<Recipe> _recipes = new List<Recipe>();
        public List<Recipe> GetAllRecipes() => _recipes;
        public List<Recipe> GetUserRecipes(string username) => _recipes.Where(recipes => recipes.Owner == username).ToList();
        public void AddRecipe(Recipe recipe)
        {
            _recipes.Add(recipe);
        }
        public void RemoveRecipe(Recipe recipe)
        {
            _recipes.Remove(recipe);
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
