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

    internal class AdminUser : User // Barnklass till Föräldraklassen User 
    {
        // FÅR ATTRIBUT FRÅN FÖRÄLDRAKLASS (User) 

        // METODER 
        public void DisplayAllRecipes()
        {

        }
        public void DeleteRecipes()
        {

        }
        public void ManageUsers()
        {

        }
    }
}
