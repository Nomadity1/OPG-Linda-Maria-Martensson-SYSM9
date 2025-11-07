using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    public  class Recipe
    {
        // PUBLIKA EGENSKAPER
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string CookingTime { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }
        public DateTime Date { get; set; } // Datummetod för när receptet skapas

        // Readonly omvandling av datum-tid-funktion för att kunna binda till UI't
        public string DateString => Date.ToString("g"); // short date + time


        // KONSTRUKTOR för att initiera ett Recipe-objekt
        public Recipe(string title, string ingredients, string instructions, string cookingTime, string category, string owner)
        {
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            CookingTime = cookingTime; 
            Category = category;
            Owner = owner;
            Date = DateTime.Now;
        }
    }
}