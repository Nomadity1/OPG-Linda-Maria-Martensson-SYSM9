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
    // KLASS SOM SKA HANTERA INLOGGAD ANVÄNDARE
    // FÄLT OCH EGENSKAPER - Samarbetar med UserManagerViewModel i ViewModelsmappen

    // KLASS SOM SKA HANTERA INLOGGAD ANVÄNDARE och se till att VI HAR SAMMA TILLSTÅND I HELA PROJEKTET 
    // SKA HANTERA LOG IN OCH LOG OUT (METODER genom COMMANDS) 
    internal class UserManager : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // ATTRIBUT: PRIVATA FÄLT som använder sig av User-klassen
        private User? _currentUser; // Variabel för enskild användare - Frågetecknet anger att variabeln kan ha null-värde 
        private List<User> _userlist; // Lista över alla användare 

        // ATTRIBUT: PUBLIK EGENSKAP för inloggad användare
        public User CurrentUser
        {
            get { return _currentUser; }
            private set
            {
                // Det är CurrentUser som ska ändras och ange nya tillstånd (nya objekt) i projektet
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }
        // PUBLIK BOOL för att kunna visa inloggningsstatus
        public bool IsAuthenticated => CurrentUser != null;

        // KONSTRUKTOR: Konstruerar klassen genom att skapa objektet _userlist av listan
        public UserManager()
        {
            _userlist = new List<User>();
            CreateDefaultUsers();
        }
        // Metod för att lägga till användare i lista 
        private void CreateDefaultUsers()
        {
            _userlist.Add(new User{Username = "LindaMaria", DisplayName = "Administratör", Role = "admin",Password = "0000"});
            _userlist.Add(new User{Username = "Elsa", DisplayName = "Elsa", Role = "member", Password = "0001" });
            _userlist.Add(new User{Username = "Elvis", DisplayName = "Elvis", Role = "member", Password = "0002" });
        }
        // METOD för att logga in 
        public bool Login(string username, string password)
        {
            foreach (var user in _userlist)
            {
                if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase)
                    && user.Password == password)
                {
                    CurrentUser = user;
                    return true;
                }
            }
            return false;
        }
        // METOD för att logga ut 
        public void Logout()
        {
            CurrentUser = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}