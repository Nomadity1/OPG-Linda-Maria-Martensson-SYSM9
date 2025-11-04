using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    // INSTRUKTIONER: En Admin-User ska kunna se/ta bort alla recept tillagda av alla användare
    // När programmet startar ska en Admin-User med användarnamn “admin” (små bokstäver!) och
    // lösenord “password” (små bokstäver!) finnas. FINNAS = REGISTRERAD

    public class AdminUser : User // Barnklass till Föräldraklassen User 
    {
        // ÄRVER EGENSKAPER FRÅN FÖRÄLDRAKLASS (User)
        
        // KONSTRUKTOR 
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
