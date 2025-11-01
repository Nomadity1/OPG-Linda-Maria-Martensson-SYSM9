using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    public  class Recipe
    {
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Owner { get; set; }
        public Recipe(string title, string ingredients, string instructions, string category, string owner)
        {
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            Category = category;
            Owner = owner;
            Date = DateTime.Now;
        }
    }
}
