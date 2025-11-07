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

    // UPPGIFTER är att HANTERA RECEPT (LÄGGA TILL, TA BORT, HÄMTA LISTOR)
    public class RecipeManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT för recepthanteraren 
        private List<Recipe> _recipelist; // Generell receptlista 
        private Recipe? _currentRecipe; // Enskilt recept 
        // PUBLIKA EGENSKAPER för recepthanteraren med specifika listor beroende på användare
        public List<Recipe> GetAllRecipes() => _recipelist;
        public List<Recipe> GetUserRecipes(string username) => _recipelist.Where(recipes => recipes.Owner == username).ToList();

        // Det är CurrentRecipe som ska ändras och ange nya tillstånd (nya objekt) i projektet
        public Recipe? CurrentRecipe { get { return _currentRecipe; }
            set {
                _currentRecipe = value;
                OnPropertyChanged(nameof(CurrentRecipe)); }
        }
        // PUBLIL BOOL för att hantera aktuellt recept
        public bool HasCurrentRecipe => _currentRecipe != null;

        // KONSTRUKTOR
        public RecipeManager()
        {
            // Initierar med tom lista
            _recipelist = new List<Recipe>();
            // Kollar om listan är tom
            if (!_recipelist.Any())
            {
                // Anropar i så fall metod för att skapa basanvändare
                CreateDefaultRecipes();
            }
        }
        // METOD för att skapa standardanvändare för testning och utvärdering
        private void CreateDefaultRecipes()
        {
            // Lägger till recept som administratör genom att anropa konstruktorn med argument 
            _recipelist.Add(new Recipe(     
                "Smaklösa Saras soppa",
                "Soppa på burk",
                "Öppna och värm",
                "1 h",
                "Soppa",
                "admin")
            { Date = DateTime.Now }
            );
            // Lägger till recept som vanlig användare genom att anropa basklassens konstruktorn
            _recipelist.Add(new Recipe(
                "Kalas-Kalles kolakakor",
                "Smör, socker, sirap, mjöl",
                "Blanda, forma, grädda",
                "30 min", 
                "Fika",
                "user")
            { Date = DateTime.Now }
            );
            // Lägger till recept som vanlig användare genom att anropa basklassens konstruktorn
            _recipelist.Add(new Recipe(
                "Gulliga och gröna grodlår",
                "Grodlår, gullighet, grön färg",
                "Blanda allt",
                "10 min",
                "Förrätt",
                "user")
            { Date = DateTime.Now }
            );
        }

        // METODER för recepthanteraren (LÄGG TILL/SKAPA, ÄNDRA, TA BORT)
        public (bool success, string message) AddRecipe(string title, string ingredients, string instructions, string cookingTime, string category, string owner)
        {
            // Skapar nytt receptobjekt och lägger till i listan
            var recipe = new Recipe(title, ingredients, instructions, cookingTime, category, owner) {Date = DateTime.Now };
            _recipelist.Add(recipe);
            return (true, "Receptet har sparats.");
        }
        public (bool success, string message) UpdateRecipe(Recipe recipe)
        {
            // Kontrollera om receptet finns
            if (recipe == null)
                return (false, "Receptet är null.");

            // Letar upp receptet i samlingen - TIPS FRÅN GITHUB COPILOT
            var existing = _recipelist.FirstOrDefault(r => ReferenceEquals(r, recipe));
            if (existing != null)
            {
                // Kopierar info 
                existing.Title = recipe.Title;
                existing.Ingredients = recipe.Ingredients;
                existing.Instructions = recipe.Instructions;
                existing.CookingTime = recipe.CookingTime;
                existing.Category = recipe.Category;
                existing.Date = DateTime.Now;

                // Meddelar till bindningarna att ändringar gjorts
                OnPropertyChanged(nameof(GetAllRecipes));
                OnPropertyChanged(nameof(Recipe));
                OnPropertyChanged(nameof(CurrentRecipe)); 
                return (true, "Receptet har uppdaterats.");
            }
            // Alternativplan "FALL BACK" om recept inte hittas mha titeln - TIPS FRÅN GITHUB COPILOT
            var fallback = _recipelist.FirstOrDefault(r => r.Owner == recipe.Owner && r.Date == recipe.Date && r.Title == recipe.Title);
            if (fallback != null)
            {
                fallback.Title = recipe.Title;
                fallback.Ingredients = recipe.Ingredients;
                fallback.Instructions = recipe.Instructions;
                fallback.CookingTime = recipe.CookingTime;
                fallback.Category = recipe.Category;
                fallback.Date = DateTime.Now;
                OnPropertyChanged(nameof(GetAllRecipes));
                OnPropertyChanged(nameof(Recipe));
                OnPropertyChanged(nameof(CurrentRecipe));
                return (true, "Receptet har uppdaterats (fallback match).");
            }
            return (false, "Kunde inte hitta receptet att uppdatera.");
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