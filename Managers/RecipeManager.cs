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
        // PRIVATA FÄLT för recepthanteraren 
        private List<Recipe> _recipelist; // Generell receptlista 
        private Recipe? _recipe; // Enskilt recept 
        // PUBLIKA EGENSKAPER för recepthanteraren med specifika listor beroende på användare
        public List<Recipe> GetAllRecipes() => _recipelist;
        public List<Recipe> GetUserRecipes(string username) => _recipelist.Where(recipes => recipes.Owner == username).ToList();
        public Recipe? Recipe
        {
            get => _recipe;
            set { _recipe = value; OnPropertyChanged(); }
        }
        // KONSTRUKTOR
        public RecipeManager()
        {
            // Initierar med tom lista
            _recipelist = new List<Recipe>();
            // Kollar om listan är tom
            if (!_recipelist.Any())
            {
                // Anropar i så fall metod för att skapa basanvändare
                CreateDefaultRecipe();
            }
        }
        // METOD för att skapa standardanvändare för testning och utvärdering
        private void CreateDefaultRecipe()
        {
            // Lägger till recept som administratör genom att anropa konstruktorn med argument 
            _recipelist.Add(new Recipe(     
                "Smaklösa Saras soppa",
                "Soppa på burk",
                "Öppna och värm",
                "Soppa",
                "admin")
            { Date = DateTime.Now }
            );
            // Lägger till recept som vanlig användare genom att anropa basklassens konstruktorn
            _recipelist.Add(new Recipe(
                "Kalas-Kalles kolakakor",
                "Smör, socker, sirap, mjöl",
                "Blanda, forma, grädda",
                "Fika",
                "user")
            { Date = DateTime.Now }
            );
            // Lägger till recept som vanlig användare genom att anropa basklassens konstruktorn
            _recipelist.Add(new Recipe(
                "Gulliga och goda grodlår",
                "Grodlår, gullighet, olja",
                "Massera låren ömt",
                "Förrätt",
                "user")
            { Date = DateTime.Now }
            );
        }

        // METODER för recepthanteraren (LÄGG TILL, TA BORT)
        public (bool success, string message) AddRecipe(string title, string ingredients, string instructions, string category, string owner)
        {
            // Skapar nytt receptobjekt och lägger till i listan
            var recipe = new Recipe(title, ingredients, instructions, category, owner) {Date = DateTime.Now };
            _recipelist.Add(recipe);
            return (true, "Receptet har sparats.");
        }
        public void RemoveRecipe(Recipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe), "Receptet som ska tas bort är null.");
            _recipelist.Remove(recipe);
        }
        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
