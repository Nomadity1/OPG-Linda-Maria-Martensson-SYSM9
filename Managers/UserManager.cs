using CookMaster.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
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
            CreateDefaultUsers();
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
        //PUBLIK BOOL för att kunna visa inloggningsstatus
        public bool IsAuthenticated => CurrentUser != null;

        // METOD för att lägga till användare i lista
        private void CreateDefaultUsers()
        {
            // Lägger till en administratör
            _userlist.Add(new User { UserName ="LindaMaria", Password = "Ab1!", DisplayName="Administratör", EmailAddress="lima@live.se", Role="admin" } );
            // Lägger till ytterligare två användare
            _userlist.Add(new User { UserName = "Elsa", Password = "Ab2!", DisplayName = "Elsa", EmailAddress = "elsa@live.se", Role = "Member" });
            _userlist.Add(new User { UserName = "Elvis", Password = "Ab3!", DisplayName = "Elvis", EmailAddress = "elvis@live.se", Role = "Member" });
        }

        // METOD för att logga in 
        public bool Login(string username, string password)
        {
            // Går genom lista
            foreach (var user in _userlist)
            {
                // Kollar  matchningar
                if (string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase)
                    && user.Password == password)
                {
                    // OM SANT: tilldelar user till CurrentUser
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
            // Tilldelar CurrentUser värdet null 
            CurrentUser = null;
        }
        // METOD för att validera användarnamn vid skapande av användare 
        public (bool success, string message) ValidateUsername(string username)
        {
            bool IsValidUsername = (username.Length > 3 && username.Length < 9) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidUsername ? "Användarnamnet har sparats" : "Användarnamnet ska vara mellan 4 och 8 bokstäver eller tecken långt";
            if (IsValidUsername)
            {
                string UserName = username;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera lösenord vid skapande av användare eller vid byte av lösenord
        public (bool success, string message) ValidatePassword(string password, string passwordRepeat)
        {
            // villkor för att validera nytt lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            bool IsValidPassword = (password.Length > 3 && password.Length < 9 && password.Any(char.IsUpper) && password.Any(char.IsLower)
                    && password.Any(char.IsDigit) && password.Contains(specialCharacters)) ? true : false; // Ternary sats istället för IF-sats
            string message = IsValidPassword ? "Lösenordet har sparats" : "Lösenordet ska innehålla minst 4 och max 8 bokstäver inklusive ett specialtecken och en siffra";
            if (IsValidPassword)
            {

                bool IsPasswordMatch = (password == passwordRepeat) ? true : false; // Ternary sats istället för IF-sats
                message = IsPasswordMatch ? "Lösenordet har sparats" : "Lösenorden matchar inte varandra";
                if (IsPasswordMatch)
                {
                    string Password = passwordRepeat;
                    return (true, message);
                }
                else
                {
                    return (false, message);
                }
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera e-postadress vid skapande av användare 
        public (bool success, string message) ValidateEmailAddress(string email)
        {
            bool IsValidEmailAddress = (email.Contains("@") && email.IndexOf('.') > email.IndexOf('@')) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidEmailAddress ? "E-postadressen har sparats" : "E-postadressen är ogiltig. Försök igen.";
            if (IsValidEmailAddress)
            {
                string EmailAddress = email;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}