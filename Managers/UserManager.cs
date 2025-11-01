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
    // MANAGERS Hanterar HUR DATA ANVÄNDS (snarare än definitioner av data (fält, egenskaper, (= MODELS)
    // eller hur data visas (=VIEW MODELS)).
    // MANAGERS är LÄNKEN mellan MODELS och VIEW MODELS.   
    // UPPGIFTER: KOMMANDON OCH METODER FÖR INLOGGNING, CURRENTUSER, UTLOGGNING, REGISTRERING,
    // ÄNDRA UPPGIFTER OCH ÅTERSTÄLLA LÖSENORD 
    public class UserManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT som använder sig av User-klassen
        private List<User> _userlist; // Lista över alla användare                                       
        private User? _currentUser; // Variabel för aktuell (inloggad) användare
                                    // Frågetecknet anger att variabeln kan ha null-värde 

        // PUBLIK EGENSKAP i form av Lista med fördefinierade alternativ för land 
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", 
            "New Zeeland", "Germany", "United Kingdom", "Other" };

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

        // KONSTRUKTOR: Konstruerar klassen genom att instansiera listan och skapa objektet _userlist
        public UserManager()
        {
            // Initierar listan över användare
            _userlist = new List<User>();
            // Kollar om listan är tom
            if (!_userlist.Any())
            {
                // Anropar i så fall metod för att skapa basanvändare
                CreateDefaultUsers();
            }
        }

        // METOD för att skapa standardanvändare för testning och utvärdering
        private void CreateDefaultUsers()
        {
            // Lägger till administratör genom att anropa konstruktorn med argument 
            _userlist.Add(new AdminUser { UserName = "admin", Password = "password", 
                PasswordRepeat = "password", Email = "adminuser@live.se", Country = "Sweden" });
            // Lägger till användare genom konstruktorn 
            _userlist.Add(new User { UserName = "user", Password = "password", 
                PasswordRepeat = "password", Email = "user@live.se", Country = "Sweden" });
        }

        // METOD för att logga in (autentisering)
        public bool ValidateLogin(string username, string password)
        {

            // Funktion för att undvika att jag får svar (felmeddelande) vid första bästa träff 
            // ... Vill gå vidare i valideringen
            var user = _userlist.FirstOrDefault(user => string.Equals(user.UserName, username, 
                StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                // OM användaruppgifter INTE finns i listan
                MessageBox.Show("Användarnamnet finns inte");
                return false;
            }
            if (user.Password != password)
            {
                MessageBox.Show("Fel lösenord");
                return false;
            }
            // ANNARS  - användaruppgifter finns i listan OCH lösenordet matchar
            // tilldelar user till CurrentUser
            CurrentUser = user;

            // stänger MainWindow
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.Close();
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window is MainWindow)
            //    {
            //        window.Close();
            //        break;
            //    }
            // ...och visar receptvyn
            // Instansierar och visar receptvyn
            var recipeList = new RecipeListWindow();
            recipeList.ShowDialog();
            // och hälsar viewmodel att login är successful!
            return true;
        }
        // METOD för utloggning
        public void Logout()
        {
            CurrentUser = null;
            // Instansierar inloggnignsvyn
            var mainWindow = new MainWindow();
            // stänger Receptvyn
            foreach (Window window in Application.Current.Windows)
            {
                if (window is RecipeListWindow)
                {
                    window.Close();
                    break;
                }
                // ...och visar den
                mainWindow.ShowDialog();
            }
        }

        // METOD för att öppna registreringsfönster
        public void OpenRegisterWindow()
        {
            // Instansierar registreringsfönster
            var registerWindow = new RegisterWindow();
            // Visar registreringsfönster
            registerWindow.ShowDialog();
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
            user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
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
                // Skapar ett nytt användarobjekt
                var user = new User { UserName = username, Password = newPassword, PasswordRepeat = repeatPassword, Email = email, Country = country };
                // Skapar en egenskap för land
                var setCountry = user.GetType();
                var CountryProperty = setCountry.GetProperty("Countries", BindingFlags.Public | BindingFlags.Instance);
                // Lägg till i lista över användare
                _userlist.Add(user);
                // Instansierar inloggnignsvyn
                var mainWindow = new MainWindow();
                // stänger denna vyn
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is RegisterWindow)
                    {
                        window.Close();
                        break;
                    }
                    // ...och visar den
                    mainWindow.ShowDialog();
                }
                // Meddela framgång
                return (true, messageRegistration);
            }
        }

        // METOD för att återställa glömt lösenord
        public bool ResetPassword(string username, string reply)
        {
            // Går genom lista
            foreach (var user in _userlist)
            {
                // Kollar  matchningar
                if (!string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    MessageBox.Show($"Vilka är alfabetets tre första bokstäver?");
                }
            }
            if (reply != "abc")
            {
                    return false;
            }
            else if (reply == "abc")
            {
                // Instansierar userdetails-fönster
                UserDetailsWindow userDetailsWindow = new UserDetailsWindow();
                // Visar userdetails-fönster
                userDetailsWindow.ShowDialog();
                // stäng Main (denna window är troligen MainWindow)
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow)
                    {
                        window.Close();
                        break;
                    }
                }
                // ...och visar den
                userDetailsWindow.ShowDialog();
            }
            return true;
        }
        public (bool success, string message) UserNameUpdate(User updated)
        {
            bool updateSuccess = false; 
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";

            // Korta IF-satser för kontroll
            // Alla fält är ifyllda? 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.Email) || !string.IsNullOrWhiteSpace(updated.Password) || !string.IsNullOrWhiteSpace(updated.PasswordRepeat))
                return (false, "Alla fält måste fyllas i.");

            // Användaruppgift ledig?
            if (_userlist.Any(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase)))
                return (false, "Användarnamnet är upptaget");

            // Uppfyller kraven? 
            if (updated.UserName.Length > 3 && updated.UserName.Length < 9)
                return (false, "Användarnamnet är ogiltigt");
            else
            {
                // Grundantagande: updated user != user
                // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
                var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
                if (userIndex >= 0)
                {
                    _userlist[userIndex] = updated;
                }
            }
            // Meddela framgång
            return (true, messageUpdate);
        }
        public (bool success, string message) EmailUpdate(User updated)
        {
            bool updateSuccess = false;
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";
            var email = updated.Email;

            // Korta IF-satser för kontroll
            // Fält ifyllda? 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.Email) || !string.IsNullOrWhiteSpace(updated.Password) || !string.IsNullOrWhiteSpace(updated.PasswordRepeat))
                return (false, "Alla fält måste fyllas i.");

            // Användaruppgift ledig?
            if (_userlist.Any(user => user.Email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase)))
                return (false, "Epostadressen är redan registrerad");

            // Uppfyller kraven? 
            // Validera e-postadress 
            if (email.Contains("@") && email.IndexOf('.') > email.IndexOf('@'))
                return (false, "E-postadressen är ogiltig");

            // Kontrollera att land valts
            if (!string.IsNullOrWhiteSpace(updated.Country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Grundantagande: updated user != user
                // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
                var userIndex = _userlist.FindIndex(user => user.Email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase));
                if (userIndex >= 0)
                {
                    _userlist[userIndex] = updated;
                }
            }
            // Meddela framgång
            return (true, messageUpdate);
        }
        public (bool success, string message) PasswordUpdate(User updated)
        {
            bool updateSuccess = false;
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";

            // Korta IF-satser för kontroll
            // Fält ifyllda? 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.Email) || !string.IsNullOrWhiteSpace(updated.Password) || !string.IsNullOrWhiteSpace(updated.PasswordRepeat))
                return (false, "Alla fält måste fyllas i.");

            // Validera lösenord
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            if (updated.Password.Length > 4 && updated.Password.Length < 9 && updated.Password.Any(char.IsUpper) && updated.Password.Any(char.IsLower)
                    && updated.Password.Any(char.IsDigit) && updated.Password.Contains(specialCharacters))
                return (false, "Lösenordet är ogiltig");

            // Validera upprepat lösenord
            if (updated.Password != updated.PasswordRepeat)
                return (false, "Lösenorden matchar inte varandra.");

            // Kontrollera att land valts
            if (!string.IsNullOrWhiteSpace(updated.Country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Grundantagande: updated user != user
                // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
                var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
                if (userIndex >= 0)
                {
                    _userlist[userIndex] = updated;
                }
            }
            // Meddela framgång
            return (true, messageUpdate);
        }
        public (bool success, string message) UpdateSelectedCountry(User updated)
        {
            bool updateSuccess = false;
            string messageUpdate = updateSuccess ? "Dina uppgifter har uppdaterats." : "";

            // Implementerar korta IF-satser för kontroll
            // Kontrollera att alla fält är ifyllda 
            if (!string.IsNullOrWhiteSpace(updated.UserName) || !string.IsNullOrWhiteSpace(updated.Email) || !string.IsNullOrWhiteSpace(updated.Password) || !string.IsNullOrWhiteSpace(updated.PasswordRepeat))
                return (false, "Alla fält måste fyllas i.");

            // Kontrollera att land valts
            if (!string.IsNullOrWhiteSpace(updated.Country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Grundantagande: updated user != user
                // ...så om användarnamn hittas i listan så skrivs det över med uppdaterad info
                var userIndex = _userlist.FindIndex(user => user.UserName.Equals(updated.UserName, StringComparison.OrdinalIgnoreCase));
                if (userIndex >= 0)
                {
                    _userlist[userIndex] = updated;
                }
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