using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    internal class User
    {
        // Auto-implementerade PUBLIKA EGENSKAPER som definierar en enskild användare 
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        // KONSTRUKTOR - måste ha konstruktor för att kunna implementera barnklassen AdminUser? 
        public User(string UserName, string DisplayName, string Role, string PassWord)
        {
            this.Username = UserName;
            this.DisplayName = DisplayName;
            this.Role = Role;
            this.Password = PassWord;
        }
        // METOD för att validera lösenord vid skapande av användare 

    }
}
