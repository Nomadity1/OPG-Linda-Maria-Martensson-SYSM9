using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    internal class AdminUser : User // Barnklass till Föräldraklassen User 
    {
        // FÅR ATTRIBUT FRÅN FÖRÄLDRAKLASS (User) 
        // KONSTRUKTOR anropar föräldraklassens konstruktor 
        public AdminUser(string UserName, string DisplayName, string Role, string PassWord) : base(UserName, DisplayName, Role, PassWord)
        {
            this.Username = UserName;
            this.DisplayName = DisplayName;
            this.Role = Role;
            this.Password = PassWord;
        }
    }
}
