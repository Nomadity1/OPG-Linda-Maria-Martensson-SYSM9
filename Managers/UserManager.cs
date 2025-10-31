using CookMaster.Models;
using CookMaster.Views;
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

        // METOD för att skapa standardanvändare för testning och utvärdering
        private void CreateDefaultUsers()
        {
            // Lägger till administratör genom att anropa konstruktorn med argument (konstruktorn kräver parametrar)
            _userlist.Add(new AdminUser { UserName = "admin", Password = "password", PasswordRepeat = "password", EmailAddress = "adminuser@live.se", Country = "Sweden" });
            // Lägger till användare genom konstruktorn 
            _userlist.Add(new User { UserName = "user", Password = "password", PasswordRepeat = "password", EmailAddress = "user@live.se", Country = "Sweden" });
        }

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

        // METOD för att registrera ny användare
        public (bool success, string message) Register(string username, string email, string newPassword, string repeatPassword, string country)
        {
            bool registrationSuccess = false;
            string messageRegistration = registrationSuccess ? "Registrering lyckades. Dina uppgifter är sparade." : "";

            // Implementerar korta IF-satser för kontroll

            // Kontrollera att alla fält är ifyllda 
            if (!string.IsNullOrWhiteSpace(username) || !string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(repeatPassword))
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
                var user = new User { UserName = username, Password = newPassword, PasswordRepeat = "", EmailAddress = email, Country = country };
                // Skapar en egenskap för land
                var setCountry = user.GetType();
                var CountryProperty = setCountry.GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance);
                // Lägg till i lista över användare
                _userlist.Add(user); 
            }
            // Meddela framgång
            return (true, messageRegistration);
        }

        // METOD för att återställa glömt lösenord
        public bool ResetPassword(string username, string reply)
        {
            // Register(string username, string email, string newPassword, string repeatPassword, string country)
            
            // Går genom lista
            foreach (var user in _userlist)
            {
                // Kollar  matchningar
                if (string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Vilka är alfabetets tre första bokstäver?");
                    if (reply != "abc")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            // ANNARS: felmeddelande
            return false;
        }
        public (bool success, string message) UserNameUpdate(User updated)
        {
            bool updateSuccess = false; 
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";

            // Grundantagande: updated user != user
            // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
            var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            if (userIndex >= 0)
            {
                _userlist[userIndex] = updated;
            }
            // Korta IF-satser för kontroll
            // Alla fält är ifyllda? 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.EmailAddress) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(repeatPassword))
                return (false, "Alla fält måste fyllas i.");

            // Användarnamn ledigt?
            if (_userlist.Any(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase))
                return (false, "Användarnamnet är upptaget");

            // Uppfyller kraven? 
            if (updated.UserName.Length > 3 && updated.UserName.Length < 9)
                return (false, "Användarnamnet är ogiltigt");
            else
            {
                // Tilldelar nytt värde? 

            }
            // Meddela framgång
            return (true, messageUpdate);
        }
        public (bool success, string message) UpdateEmail(User updated)
        {
            bool updateSuccess = false;
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";
            var email = updated.EmailAddress;

            // Grundantagande: updated user != user
            // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
            var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            if (userIndex >= 0)
            {
                _userlist[userIndex] = updated;
            }

            // Korta IF-satser för kontroll
            // Fält ifyllda? 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.EmailAddress) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(repeatPassword))
                return (false, "Alla fält måste fyllas i.");

            // Kontroll om användare (användaruppgifter) redan finns i lista över användare
            if (_userlist.Any(user => user.EmailAddress.Equals(updated.EmailAddress, StringComparison.OrdinalIgnoreCase)))
                return (false, "Epostadressen är redan registrerad");

            // Validera användarnamn 
            if (updated.UserName.Length > 3 && updated.UserName.Length < 9)
                return (false, "Användarnamnet är ogiltigt");

            // Validera lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            if (updated.Password.Length > 4 && updated.Password.Length < 9 && updated.Password.Any(char.IsUpper) && updated.Password.Any(char.IsLower)
                    && updated.Password.Any(char.IsDigit) && updated.Password.Contains(specialCharacters))
                return (false, "Lösenordet är ogiltig");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera e-postadress 
            if (updated.EmailAddress.Contains("@") && updated.EmailAddress.IndexOf('.') > updated.EmailAddress.IndexOf('@'))
                return (false, "E-postadressen är ogiltig");

            // Kontrollera att land valts
            if (!string.IsNullOrWhiteSpace(updated.Country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Skapar ett användarobjekt
                var user = new User { UserName = username, Password = newPassword, PasswordRepeat = "", EmailAddress = email, Country = country };
                // Skapar en egenskap för land
                var setCountry = user.GetType();
                var CountryProperty = setCountry.GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance);
                // Lägg till i lista över användare
                _userlist.Add(user);
            }
            // Meddela framgång
            return (true, messageUpdate);
        }
        public (bool success, string message) UpdateDetails()
        {
            bool updateSuccess = false;
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";
            var username = _
                UpdatedUserName;
            var email = updated.EmailAddress;
            var newPassword = Password;
            var repeatPassword = updated.PasswordRepeat;
            var country = updated.Country;

            // Grundantagande: updated user != user
            // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
            //var user = _userlist.FirstOrDefault(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            if (userIndex >= 0)
            {
                _userlist[userIndex] = updated;
            }

            //string username, string email, string newPassword, string repeatPassword, string country
            // Implementerar korta IF-satser för kontroll
            // Kontrollera att alla fält är ifyllda 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.EmailAddress) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(repeatPassword))
                return (false, "Alla fält måste fyllas i.");

            // Kontroll om användare (användaruppgifter) redan finns i lista över användare
            if (_userlist.Any(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase) ||
            user.EmailAddress.Equals(updated.EmailAddress, StringComparison.OrdinalIgnoreCase)))
                return (false, "Användarnamnet eller epostadressen är redan registrerad");

            // Validera användarnamn 
            if (updated.UserName.Length > 3 && updated.UserName.Length < 9)
                return (false, "Användarnamnet är ogiltigt");

            // Validera lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            if (updated.Password.Length > 4 && updated.Password.Length < 9 && updated.Password.Any(char.IsUpper) && updated.Password.Any(char.IsLower)
                    && updated.Password.Any(char.IsDigit) && updated.Password.Contains(specialCharacters))
                return (false, "Lösenordet är ogiltig");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera e-postadress 
            if (updated.EmailAddress.Contains("@") && updated.EmailAddress.IndexOf('.') > updated.EmailAddress.IndexOf('@'))
                return (false, "E-postadressen är ogiltig");

            // Kontrollera att land valts
            if (!string.IsNullOrWhiteSpace(updated.Country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Skapar ett användarobjekt
                var user = new User { UserName = username, Password = newPassword, PasswordRepeat = "", EmailAddress = email, Country = country };
                // Skapar en egenskap för land
                var setCountry = user.GetType();
                var CountryProperty = setCountry.GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance);
                // Lägg till i lista över användare
                _userlist.Add(user);
            }
            // Meddela framgång
            return (true, messageUpdate);
        }
        public (bool success, string message) UpdateDetails()
        {
            bool updateSuccess = false;
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";
            var username = _
                UpdatedUserName;
            var email = updated.EmailAddress;
            var newPassword = Password;
            var repeatPassword = updated.PasswordRepeat;
            var country = updated.Country;

            // Grundantagande: updated user != user
            // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
            //var user = _userlist.FirstOrDefault(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            if (userIndex >= 0)
            {
                _userlist[userIndex] = updated;
            }

            //string username, string email, string newPassword, string repeatPassword, string country
            // Implementerar korta IF-satser för kontroll
            // Kontrollera att alla fält är ifyllda 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.EmailAddress) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(repeatPassword))
                return (false, "Alla fält måste fyllas i.");

            // Kontroll om användare (användaruppgifter) redan finns i lista över användare
            if (_userlist.Any(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase) ||
            user.EmailAddress.Equals(updated.EmailAddress, StringComparison.OrdinalIgnoreCase)))
                return (false, "Användarnamnet eller epostadressen är redan registrerad");

            // Validera användarnamn 
            if (updated.UserName.Length > 3 && updated.UserName.Length < 9)
                return (false, "Användarnamnet är ogiltigt");

            // Validera lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            if (updated.Password.Length > 4 && updated.Password.Length < 9 && updated.Password.Any(char.IsUpper) && updated.Password.Any(char.IsLower)
                    && updated.Password.Any(char.IsDigit) && updated.Password.Contains(specialCharacters))
                return (false, "Lösenordet är ogiltig");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera e-postadress 
            if (updated.EmailAddress.Contains("@") && updated.EmailAddress.IndexOf('.') > updated.EmailAddress.IndexOf('@'))
                return (false, "E-postadressen är ogiltig");

            // Kontrollera att land valts
            if (!string.IsNullOrWhiteSpace(updated.Country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Skapar ett användarobjekt
                var user = new User { UserName = username, Password = newPassword, PasswordRepeat = "", EmailAddress = email, Country = country };
                // Skapar en egenskap för land
                var setCountry = user.GetType();
                var CountryProperty = setCountry.GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance);
                // Lägg till i lista över användare
                _userlist.Add(user);
            }
            // Meddela framgång
            return (true, messageUpdate);
        }
        public (bool success, string message) UpdateDetails()
        {
            bool updateSuccess = false;
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";
            var username = _
                UpdatedUserName;
            var email = updated.EmailAddress;
            var newPassword = Password;
            var repeatPassword = updated.PasswordRepeat;
            var country = updated.Country;

            // Grundantagande: updated user != user
            // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
            //var user = _userlist.FirstOrDefault(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
            if (userIndex >= 0)
            {
                _userlist[userIndex] = updated;
            }

            //string username, string email, string newPassword, string repeatPassword, string country
            // Implementerar korta IF-satser för kontroll
            // Kontrollera att alla fält är ifyllda 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.EmailAddress) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(repeatPassword))
                return (false, "Alla fält måste fyllas i.");

            // Kontroll om användare (användaruppgifter) redan finns i lista över användare
            if (_userlist.Any(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase) ||
            user.EmailAddress.Equals(updated.EmailAddress, StringComparison.OrdinalIgnoreCase)))
                return (false, "Användarnamnet eller epostadressen är redan registrerad");

            // Validera användarnamn 
            if (updated.UserName.Length > 3 && updated.UserName.Length < 9)
                return (false, "Användarnamnet är ogiltigt");

            // Validera lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            if (updated.Password.Length > 4 && updated.Password.Length < 9 && updated.Password.Any(char.IsUpper) && updated.Password.Any(char.IsLower)
                    && updated.Password.Any(char.IsDigit) && updated.Password.Contains(specialCharacters))
                return (false, "Lösenordet är ogiltig");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Validera e-postadress 
            if (updated.EmailAddress.Contains("@") && updated.EmailAddress.IndexOf('.') > updated.EmailAddress.IndexOf('@'))
                return (false, "E-postadressen är ogiltig");

            // Kontrollera att land valts
            if (!string.IsNullOrWhiteSpace(updated.Country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Skapar ett användarobjekt
                var user = new User { UserName = username, Password = newPassword, PasswordRepeat = "", EmailAddress = email, Country = country };
                // Skapar en egenskap för land
                var setCountry = user.GetType();
                var CountryProperty = setCountry.GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance);
                // Lägg till i lista över användare
                _userlist.Add(user);
            }
            // Meddela framgång
            return (true, messageUpdate);
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}