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
    // INSTRUKTIONER: SKA HANTERA INLOGGAD ANVÄNDARE, DEFINIERA CURRENT USER, samt
    // METODER OCH KOMMANDON FÖR INLOGGNING, UTLOGGNING, REGISTRERING, CHANGEPASSWORD, CURRENTUSER
    public class UserManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT som använder sig av User-klassen
        private User? _currentUser; // Variabel för enskild användare - Frågetecknet anger att variabeln kan ha null-värde 
        private List<User> _userlist; // Lista över alla användare                                       
        
        // PUBLIK EGENSKAP i form av Lista med fördefinierade alternativ för land 
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", "New Zeeland", "Germany", "United Kingdom", "Other" };

        // KONSTRUKTOR: Konstruerar klassen genom att skapa objektet _userlist av listan och 
        public UserManager()
        {
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

        // METOD för att lägga till/spara användare i lista
        private void CreateDefaultUsers()
        {
            // Lägger till administratör
            //_userlist.Add(new User { UserName ="LindaMaria", Password = "Ab1!", DisplayName="Administratör", EmailAddress="lima@live.se", Role="admin", PinCode="0001" } );
            _userlist.Add(new User { UserName = "adminuser", EmailAddress = "adminuser@live.se", Password = "password", DisplayName = "Administratör", Role = "admin", PinCode = "0000", Country="Sweden" });
            // Lägger till ytterligare två användare
            _userlist.Add(new User { UserName = "user", EmailAddress = "user@live.se", Password = "password", DisplayName = "Användare", Role = "user", PinCode = "0000", Country = "Sweden" });
            //_userlist.Add(new User { UserName = "Elsa", Password = "Ab2!", DisplayName = "Elsa", EmailAddress = "elsa@live.se", Role = "Member", PinCode = "0002" });
            //_userlist.Add(new User { UserName = "Elvis", Password = "Ab3!", DisplayName = "Elvis", EmailAddress = "elvis@live.se", Role = "Member", PinCode = "0003" });    
        }

        // METOD för att logga in (autentisering)
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
        // Öppna registreringsfönster
        public void GoToSignUp()
        {
            // Instansierar och visar registrerings-fönstret genom objektet registerWindow
            //RegisterWindow registerWindow = new RegisterWindow();
            //registerWindow.Show();
        }

        // METOD för att registrera ny användare
        public bool Register(User newUser)
        {
            bool userAlreadyListed = _userlist.Any(user => user.UserName.Equals(newUser.UserName, StringComparison.OrdinalIgnoreCase) ||
            user.EmailAddress.Equals(newUser.EmailAddress, StringComparison.OrdinalIgnoreCase));
            // Kolla  om användarnman eller epost redan finns i lista över användare
            if (userAlreadyListed) // villkor
            {
                // OM SANT: felmeddelande
                return false;
            }
            else
            {
                // ANNARS: Lägg till i lista över användare
                _userlist.Add(newUser);
                // Ge standardtilldelning av roll (kommer ju alltid att vara tom så som jag riggat inmatning)
                if (string.IsNullOrEmpty(newUser.Role))
                    newUser.Role = "Member";
                // Och meddela att registrering lyckats
                return true;
            }
        }

        // METOD för att ta emot begäran om att återställa glömt lösenord
        // Egentligen: Ta emot request, hitta epost i lista, skapa pin, skicka pin med epost och spara tillfälligt i användaren
        // Här nöjer jag mig med att kolla av om pin matchar med användare och skicka tillåtelse att komma åt UserDetails, där password kan ändras
        public bool RequestPincode(string email)
        {
            // Går genom lista
            foreach (var user in _userlist)
            {
                // Kollar  matchningar
                if (string.Equals(user.EmailAddress, email, StringComparison.OrdinalIgnoreCase))
                {
                    // OM SANT: meddelade/tillstånd att ändra lösenord i UserDetails
                    return true;
                }
            }
            // ANNARS: felmeddelande
            return false;
        }
        // METOD för att ÄNDRA lösenord (oavsett om det är pga glömt lösen eller annan anledning)
        public bool ChangePassword(string pin)
        {
            // Går genom lista över registrerade användare
            foreach (var user in _userlist)
            {
                // Kollar  matchningar
                if (user.PinCode == pin) // villkor
                {
                    // OM SANT: skickar meddelade/tillåter ändra lösenord i UserDetails
                    return true;
                }
            }
            // ANNARS: Felmeddelande
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