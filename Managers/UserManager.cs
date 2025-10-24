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
    // GENERELLT OM MANAGERS: Hanterar HUR DATA ANVÄNDS (snarare än definitioner av data (fält, egenskaper,
    // validering av inmatning (= MODELS) eller hur data visas (=VIEW MODELS)).
    // MANAGERS är LÄNKEN mellan MODELS och VIEW MODELS. 
    // INSTRUKTIONER: SKA HANTERA INLOGGAD ANVÄNDARE, DEFINIERA CURRENT USER, samt METODER OCH KOMMANDON
    // FÖR INLOGGNING och UTLOGGNING
    public class UserManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT som använder sig av User-klassen
        private User? _currentUser; // Variabel för enskild användare - Frågetecknet anger att variabeln kan ha null-värde 
        private List<User> _userlist; // Lista över alla användare 

        // KONSTRUKTOR: Konstruerar klassen genom att skapa objektet _userlist av listan
        public UserManager()
        {
            _userlist = new List<User>();
            CreateDefaultUser();
        }

        // PUBLIK EGENSKAP för inloggad användare
        public User? CurrentUser
        {
            get { return _currentUser; }
            private set
            {
                // Det är CurrentUser som ska ändras och ange nya tillstånd (nya objekt) i projektet
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser)); // för att visa vem
                OnPropertyChanged(nameof(IsAuthenticated)); // för att visa status 
            }
        }
        // Metod för att lägga till en enda användare i lista, kan sen användas för att lägga till fler användare 
        private void CreateDefaultUser()
        {
            User user = new User("LindaMaria", "Administratör", "lima@live.se", "admin", "0000");
            _userlist.Add(user);
            // Lägger till ytterligare två användare
            _userlist.Add(new User("Elsa", "Elsa", "elsa@live.se", "member", "0001"));
            _userlist.Add(new User("Elvis", "Elvis", "elvis@live.se", "member", "0002"));
        }

        //PUBLIK BOOL för att kunna visa inloggningsstatus
        public bool IsAuthenticated => CurrentUser != null;

        // METOD för att logga in 
        public bool Login(string username, string password)
        {
            // Går genom lista
            foreach (var user in _userlist)
            {
                // Kollar av matchningar
                if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase)
                    && user.Password == password)
                {
                    // OM SANT: tilldelar currentuser
                    CurrentUser = user;
                    return true;
                }
            }
            // Annars: felmeddelande
            return false;
        }
        // METOD för att logga ut 
        public void Logout()
        {
            // Behöver ingen okontroll, stänger bara huvud-vyn (Recipe List) och öppnar splash-vyn (LogIn)
            // Tilldelar currentuserr värdet null 
            CurrentUser = null;
        }
        // METOD för att spara ny användare
        //private void AddUser(User newUser)
        //{

        //    User newUser = new User({ Username }, 
        //    newUser.EmailAddress; 
        //        string UserName, string DisplayName, string EmailAddress, string Role, string PassWord)

        //    User user = new User("LindaMaria", "Administratör", "lima@live.se", "admin", "0000");
        //    _userlist.Add(user);
        //    // Lägger till ytterligare två användare
        //    _userlist.Add(new User("Elsa", "Elsa", "elsa@live.se", "member", "0001"));
        //    _userlist.Add(new User("Elvis", "Elvis", "elvis@live.se", "member", "0002"));
        //}

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}