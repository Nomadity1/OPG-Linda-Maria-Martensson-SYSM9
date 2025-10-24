using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Models
{
    // INSTRUKTIONER: En Admin-User ska kunna se/ta bort alla recept tillagda av alla användare
    // När programmet startar ska en Admin-User med användarnamn “admin” (små bokstäver!) och
    // lösenord “password” (små bokstäver!) finnas.
    // VAD MENAS MED FINNAS? = VARA INLOGGAD ELLER REGISTRERAD? 
    internal class AdminUser : User // Barnklass till Föräldraklassen User 
    {
        // FÅR ATTRIBUT FRÅN FÖRÄLDRAKLASS (User) 
        // KONSTRUKTOR anropar föräldraklassens konstruktor (vilket då kräver att User har en konstruktor!) 
        //public AdminUser(string UserName, string DisplayName, string EmailAddress, string Role, string PassWord) 
        //    : base(UserName, DisplayName, EmailAddress, Role, PassWord)
        //{
        //    this.Username = UserName;
        //    this.DisplayName = DisplayName;
        //    this.EmailAddress = EmailAddress;
        //    this.Role = Role;
        //    this.Password = PassWord;
        //}
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
