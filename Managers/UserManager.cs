using CookMaster.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CookMaster.Managers
{
    // GENERELLT OM MANAGERS: Hanterar HUR DATA ANVÄNDS (snarare än definitioner av data (fält, egenskaper, (= MODELS) eller hur data visas (=VIEW MODELS)).
    // MANAGERS är LÄNKEN mellan MODELS och VIEW MODELS.   
    // METODER OCH KOMMANDON FÖR INLOGGNING (inkl. validering av inmatning), CURRENTUSER, UTLOGGNING, REGISTRERING (inkl. validering av inmatning), CHANGEPASSWORD
    public class UserManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT som använder sig av User-klassen
        private User? _currentUser; // Variabel för enskild användare - Frågetecknet anger att variabeln kan ha null-värde 
        private List<AdminUser> _adminlist; // Lista över alla admin-användare                                       
        private List<User> _userlist; // Lista över alla användare                                       

        // PUBLIK EGENSKAP i form av Lista med fördefinierade alternativ för land 
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", "New Zeeland", "Germany", "United Kingdom", "Other" };

        // KONSTRUKTOR: Konstruerar klassen genom att skapa objektet _userlist av listan och 
        public UserManager()
        {
            _adminlist = new List<AdminUser>();
            // Initierar listan över användare
            _userlist = new List<User>();
            // Anropar metod för att skapa basanvändare
            CreateDefaultUsers();
        }

        // PUBLIK (EGENSKAP) DEFINITION av inloggad användare
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

        // METOD för att logga in (autentisering)
        public bool ValidateLogin(string username, string password)
        { 
            // Går genom lista
            foreach (var user in _userlist)
            {
                // Kollar  matchningar
                if (!string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase)
                    || user.Password != password)
                {
                    // OM SANT (användaruppgifter finns inte i listan)
                    return false;
                }
                // ANNARS: tilldelar user till CurrentUser
                CurrentUser = user;
            }
            // och hälsar viewmodel att login är successful!
            return true;
        }
        // Metod för utloggning
        public void Logout()
        {
            CurrentUser = null;
        }

        // METOD för att skapa standardanvändare för testning och utvärdering
        private void CreateDefaultUsers()
        {
            // Lägger till administratör genom att anropa konstruktorn med argument (konstruktorn kräver parametrar)
            _userlist.Add(new AdminUser { UserName = "admin", Password = "password", EmailAddress = "adminuser@live.se", Country = "Sweden" });
            //_adminlist.Add(new User { UserName = "admin", Password = "password", EmailAddress = "adminuser@live.se", Country = "Sweden" });
            // Lägger till användare genom konstruktorn 
            _userlist.Add(new User { UserName = "user", Password = "password", EmailAddress = "user@live.se", Country = "Sweden" });
        }

        // METOD för att registrera ny användare
        public (bool success, string message) Register(string username, string email, string newPassword, string repeatPassword, string country)
        {
            bool registrationSuccess = false;
            string messageRegistration = registrationSuccess ? "Registrering lyckades. Dina uppgifter är sparade." : "";

            // Implementerar korta IF-satser för kontroll

            // Kontrollera att alla fält är ifyllda 
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(repeatPassword))
                return (false, "Alla fält måste fyllas i.");

            // Kontroll om användare (användaruppgifter) redan finns i lista över användare
            if (_userlist.Any(user => user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) ||
            user.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return (false, "Användarnamnet eller epostadressen är redan registrerad");

            // Validera användarnamn 
            if (username.Length > 3 && username.Length < 9) 
                return (false, "Användarnamnet är ogiltigt");

            // Validera lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            if (newPassword.Length > 4 && newPassword.Length < 9 && newPassword.Any(char.IsUpper) && newPassword.Any(char.IsLower)
                    && newPassword.Any(char.IsDigit) && newPassword.Contains(specialCharacters))
                return (false, "Lösenordet är ogiltig");

            // Validera upprepat lösenord
            if (newPassword != repeatPassword)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera e-postadress 
            if (email.Contains("@") && email.IndexOf('.') > email.IndexOf('@'))
                return (false, "E-postadressen är ogiltig");

            // Kontrollera att land valts
            if (string.IsNullOrWhiteSpace(country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Skapar ett användarobjekt
                var user = new User { UserName = username, Password = newPassword, EmailAddress = email, Country = country };
                // Skapar en egenskap för land
                var setCountry = user.GetType();
                var CountryProperty = setCountry.GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance);
                // Lägg till i lista över användare
                _userlist.Add(user); 
            }
            // Meddela framgång
            return (true, messageRegistration);
        }

        // METOD för att ta emot begäran om att återställa glömt lösenord
        // Egentligen: Ta emot request, hitta epost i lista, skapa pin, skicka pin med epost och spara tillfälligt i användaren
        // Här nöjer jag mig med att kolla av om pin matchar med användare och skicka tillåtelse att komma åt UserDetails, där password kan ändras
        //public bool RequestPincode(string email)
        //{
        //    // Går genom lista
        //    foreach (var user in _userlist)
        //    {
        //        // Kollar  matchningar
        //        if (string.Equals(user.EmailAddress, email, StringComparison.OrdinalIgnoreCase))
        //        {
        //            // Skapa en låtsas-pinkod
        //            var random = new Random();
        //            string pin = random.Next(000001, 999999).ToString(); // t.ex. "4723"
        //            user.PinCode = pin;
        //            // Ska egenltigen skicka pinkoden till t.ex. epost
        //            // ...fast här gör jag det som ett popup-fönster istället! 
        //            MessageBox.Show($"Pinkod skickad till {email}: {pin}"); 
        //            return true;
        //        }
        //    }
        //    // ANNARS: felmeddelande
        //    return false;
        //}
        // METOD för att ÄNDRA lösenord (oavsett om det är pga glömt lösen eller annan anledning)
        //public bool ResetPassword(string pin)
        //{
            //// Går genom lista över registrerade användare
            //foreach (var user in _userlist)
            //{
            //    // Kollar  matchningar
            //    if (user.PinCode == pin) // villkor
            //    {
            //        // OM SANT: skickar meddelade/tillåter ändra lösenord i UserDetails
            //        return true;
            //    }
            //}
            //// ANNARS: Felmeddelande
            //return false;
        //}
        public bool ChangePassword(string newPassword, string repeatPassword)
        {
            // code to come... 
            return false; 
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}