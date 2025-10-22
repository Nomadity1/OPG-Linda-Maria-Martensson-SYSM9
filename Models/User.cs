using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    internal class User
    {
        // PUBLIKA EGENSKAPER som definierar en enskild användare 
        // Auto-implementerade 
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        // KONSTRUKTOR
        //public User(string UserName, string DisplayName, string Role, string PassWord)
        //{
        //    this.Username = UserName;
        //    this.DisplayName = DisplayName;
        //    this.Role = Role;
        //    this.Password = PassWord;
        //}
        // METOD - ingen metod? 
    }
}
