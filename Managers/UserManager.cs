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
    // MANAGERS hanterar HUR DATA ANVÄNDS, innehåller främst METODER och är LÄNKEN mellan MODELS och VIEW MODELS   
    // UPPGIFTER: INLOGGNING, CURRENTUSER, UTLOGGNING, REGISTRERING, ÄNDRA UPPGIFTER (OCH EVT. ÅTERSTÄLLA LÖSENORD) 
    public class UserManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT som använder sig av User-klassen
        private List<User> _userlist; // Lista över alla användare                                       
        private User? _currentUser; // Variabel för aktuell (inloggad) användare
                                    // Frågetecknet anger att variabeln kan ha null-värde 

        private User? _forgetfulUser; // för fejkad lösenordsförfrågan

        // PUBLIKA EGENSKAPER 
        // Det är CurrentUser som ska ändras och ange nya tillstånd (nya objekt) i projektet
        public User? CurrentUser { get { return _currentUser; } set { _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser)); // för att visa vem
                OnPropertyChanged(nameof(IsAuthenticated)); // för att visa status 
            }
        }
        public User? ForgetfulUser { get; set; }
        public List<string> Countries { get; set; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland", 
            "New Zeeland", "Germany", "United Kingdom", "Other" }; // Fördefinierade alternativ för land i registrering 
        
        // PUBLIK BOOL för att kunna visa inloggningsstatus
        public bool IsAuthenticated => CurrentUser != null; 

        // KONSTRUKTOR instansierar användarlistan med objektet _userlist
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
            _userlist.Add(new AdminUser
            {
                UserName = "admin",
                Password = "password",
                Country = "Sweden"
            });
            // Lägger till vanlig användare genom att anropa basklassens konstruktorn
            _userlist.Add(new User
            {
                UserName = "user",
                Password = "password",
                Country = "Sweden"
            });
        }
        // METOD för att skapa användare 
        private void CreateUser(string username, string password, string country)
        {
            // Skapar ny användare och tilldelar värden
            var newUser = new User { UserName = username, Password = password, Country = country };
            _userlist.Add(newUser);
        }

        // METOD som stänger alla öppna fönster (förutom eventuellt redan öppna RecipeListWindow)
        private void WindowCloser()
        {
            foreach (Window w in Application.Current.Windows.OfType<Window>().ToList())
            {
                if (!(w is RecipeListWindow))
                    w.Close();
            }
        }

        // METOD för att logga in (autentisering)
        public (bool success, string message) LogIn(string username, string password)
        {
            // Funktion för att hitta användarnamn i lista
            var user = _userlist.FirstOrDefault(u => string.Equals(u.UserName, username,
                StringComparison.OrdinalIgnoreCase));

            if (user == null) // OM användaruppgifter INTE finns i listan
            {
                //MessageBox.Show("Användarnamnet finns inte");
                return (false, "Användarnamnet finns inte.");
            }
            else if (user.Password != password) // OM lösenordet ÄR FEL 
            {
                //MessageBox.Show("Fel lösenord");
                return (false, "Fel lösenord.");
            }
            // ANNARS dvs användaruppgifter finns i listan OCH lösenordet är rätt: 
            // Tilldelar user till CurrentUser
            CurrentUser = user;
            // Meddelar framgång
            return (true, "Du har loggats in.");
        }
        // METOD för utloggning
        public void Logout()
        {
            CurrentUser = null;
            // Anropar fönsterstängare 
            WindowCloser();
            // Instansierar och visar inloggningsfönstret
            var mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }
        // METOD för att öppna registreringsfönster
        public void OpenRegisterWindow()
        {
            // Instansierar och visar registreringsfönstret
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        // METOD för att registrera ny användare
        public (bool success, string message) Register(string username, string password, string repeatPassword, string country)
        {
            // Kontroller för registrering
            if (IsUsernameTaken(username))
                return (false, "Användarnamnet är redan registrerat");
            if (!IsValidUsername(username))
                return (false, "Användarnamnet är ogiltigt");
            if (!IsValidPassword(password))
                return (false, "Lösenordet är ogiltigt");
            // Kontrollera att land valts
            if (password != repeatPassword)
                return (false, "Lösenorden matchar inte!");
            if (string.IsNullOrWhiteSpace(country))
                return (false, "Välj det land du bor i.");
            else
            {
                // Anropar metod för att skapa användare
                CreateUser(username, password, country);
                // Meddelar framgång
                return (true, "Registrering lyckades.");
            }
        }
        // METOD för att återställa lösenord (förenklad version utan e-post)
        public (bool success, string message) ForgotPassword(string username)
        {
            var user = _userlist.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                _forgetfulUser = null;
                return (false, "Användarnamnet finns inte.");
            }

            _forgetfulUser = user;
            // return the "question" text; UI decides how to present it
            return (true, "Vad är de tre första bokstäverna i alfabetet?");
        }
        public (bool success, string message) ValidateReply(string answer)
        {
            // Kontrollerar att en återställningsbegäran finns
            if (_forgetfulUser == null)
                return (false, "Ingen återställningsbegäran hittades. Ange användarnamn först.");

            // Kontrollerar svaret (fejk)
            if (string.Equals(answer, "abc", StringComparison.OrdinalIgnoreCase))
            {
                var password = _forgetfulUser.Password; // hämtar lösenordet för användarobjektet
                _forgetfulUser = null;
                return (true, $"Ditt lösenord är: {password}");
            }
            // Återställer användare
            _forgetfulUser = null;
            // meddelar fel svar
            return (false, "Fel svar.");
        }

        // METOD för att uppdatera befintlig användare
        public (bool success, string message) Update(string username, string newPassword, string repeatNewPassword, string newCountry)
        {
            if (CurrentUser == null)
            {
                return (false, "Ingen användare är inloggad.");
            }
            // Kontroller för uppdatering 
            if (IsUsernameTaken(username) && !string.Equals(username, CurrentUser.UserName, StringComparison.OrdinalIgnoreCase))
                return (false, "Användarnamnet är redan registrerat");
            if (!IsValidUsername(username))
                return (false, "Användarnamnet är ogiltigt");
            if (!IsValidPassword(newPassword))
                return (false, "Lösenordet är ogiltigt");
            if (newPassword != repeatNewPassword)
                return (false, "Lösenorden matchar inte!");
            // Kontrollera att land valts samt tillåt att det inte ändras
            if (string.IsNullOrWhiteSpace(newCountry) && !string.Equals(newCountry, CurrentUser.Country, StringComparison.OrdinalIgnoreCase))
                return (false, "Välj det land du bor i.");

            // Om användaren ändrar lösenord kallas valideringsmetod
            var isChangingPassword = !string.Equals(newPassword, CurrentUser.Password, StringComparison.Ordinal);
            if (isChangingPassword && !IsValidPassword(newPassword))
                return (false, "Lösenordet är ogiltigt");

            // Tar bort gammalt användarobjekt
            var existingUser = _userlist.FirstOrDefault(u => u.UserName.Equals(CurrentUser.UserName, StringComparison.OrdinalIgnoreCase));
            if (existingUser != null)
            {
                existingUser.UserName = username;
                existingUser.Password = newPassword;
                existingUser.Country = newCountry;

                // Sätt CurrentUser till den uppdaterade instansen
                CurrentUser = existingUser;
            }
            // Meddelar framgång
            return (true, "Dina uppgifter har uppdaterats.");
        }
        // METODER FÖR VALIDERING AV ANVÄNDARUPPGIFTER - NU UPPDELADE FÖR BÄTTRE ÖVERBLICK
        // ANVÄNDS BÅDE FÖR REGISTRERING OCH UPPDATERING 
        private bool IsUsernameTaken(string username)
        //private bool IsUsernameTaken(string username, string email)
        {
            return _userlist.Any(user => user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        private bool IsValidUsername(string username)
        {
            return username.Length >= 4 && username.Length <= 8;
        }
        private bool IsValidPassword(string password)
        {
            string specialCharacters = "!@#$%^&*()-_=+[{]};:’\"|\\,<.>/?";
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(specialCharacters.Contains);
        }
        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}