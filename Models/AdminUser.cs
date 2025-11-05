using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    // INSTRUKTIONER: En Admin-User ska kunna se/ta bort alla recept tillagda av alla användare
    public class AdminUser : User // Barnklass till Föräldraklassen User 
    {
        // ÄRVER EGENSKAPER FRÅN FÖRÄLDRAKLASS (User)

        // OVERLOADANDE KONSTRUKTORER för olika sätt att skapa ett AdminUser-objekt 
        public AdminUser() : base()
        {
        }
        public AdminUser(string userName, string password) : base(userName, password)
        {
            UserName = userName;
            Password = password;
        }
        public AdminUser(string userName, string password, string country) : base(userName, password, country)
        {
            UserName = userName;
            Password = password;
            Country = country;
        }
    }
}
