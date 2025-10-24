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
        // Metod för att lägga till en enda användare i lista, kan sen användas för att lägga till fler användare 
        private void CreateDefaultUsers()
        {
            // Lägger till en administratör
            _userlist.Add(new User { Username="LindaMaria", DisplayName="Administratör", EmailAddress="lima@live.se", Role="admin", Password="0000" } );
            // Lägger till ytterligare två användare
            _userlist.Add(new User { Username = "Elsa", DisplayName = "Elsa", EmailAddress = "elsa@live.se", Role = "member", Password = "0001" });
            _userlist.Add(new User { Username = "Elvis", DisplayName = "Elvis", EmailAddress = "elvis@live.se", Role = "member", Password = "0002" });
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
        // Auto-implementerade PUBLIKA EGENSKAPER som definierar en enskild användare 
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        // Lista med fördefinierade alternativ för land 
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", "New Zeeland", "Germany", "United Kingdom", "Other" };

        //// KONSTRUKTOR
        //public User(string UserName, string DisplayName, string EmailAddress, string Role, string PassWord)
        //{
        //    this.Username = UserName;
        //    this.DisplayName = DisplayName;
        //    this.EmailAddress = EmailAddress; 
        //    this.Role = Role;
        //    this.Password = PassWord;
        //}

        // METOD för att validera användarnamn vid skapande av användare 
        public (bool success, string message) ValidateUsername(string username)
        {
            //string message = isValidEmail ? "E-postadressen är giltig" : "E-postadressen är ogiltig. Försök igen.";
            bool IsValidUsername = (username.Length > 3 && username.Length < 9 ) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidUsername ? "Användarnamnet har sparats" : "Användarnamnet är ogiltigt. Användarnamnet ska vara mellan 4 och 8 bokstäver eller tecken långt. Försök igen.";
            if (IsValidUsername)
            {
                Username = username;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }
        // METOD för att validera e-postadress vid skapande av användare 
        public (bool success, string message) ValidateEmailAddress(string email)
        {
            //string message = isValidEmail ? "E-postadressen är giltig" : "E-postadressen är ogiltig. Försök igen.";
            bool IsValidEmailAddress = (email.Contains("@") && email.IndexOf('.') > email.IndexOf('@')) ? true : false; // Ternary sats istället för IF-sats för att kontrollera om e-postadressen är giltig
            string message = IsValidEmailAddress ? "E-postadressen har sparats" : "E-postadressen är ogiltig. Försök igen.";
            if (IsValidEmailAddress)
            {
                EmailAddress = email;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        // METOD för att validera lösenord vid skapande av användare och nytt lösenord 
        public (bool success, string message) ValidatePassWord(string password)
        {
            // villkor för att validera nytt lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            bool IsValidPassWord = (Password.Length > 4 && Password.Length < 8 && Password.Any(char.IsUpper) && Password.Any(char.IsLower)
                    && Password.Any(char.IsDigit) && Password.Contains(specialCharacters)) ? true : false; // Ternary sats istället för IF-sats
            string message = IsValidPassWord ? "Lösenordet har sparats" : "Lösenordet uppfyller inte kraven";
            if (IsValidPassWord)
            {
                Password = password;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }
        // METOD för att validera lösenord vid byte av nytt lösenord
        public (bool success, string message) ValidateRepeatedPassWord(string passwordRepeat)
        {
            // villkor för lösenordsinmatning
            bool IsPassWordMatch = (Password == passwordRepeat) ? true : false; // Ternary sats istället för IF-sats
            string message = IsPassWordMatch ? "Lösenordet har sparats" : "Lösenorden matchar inte varandra";
            if (IsPassWordMatch)
            {
                Password = passwordRepeat;
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }
        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}