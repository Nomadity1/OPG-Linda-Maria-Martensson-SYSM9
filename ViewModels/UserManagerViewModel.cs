using CookMaster.Models;
using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.ViewModels
{
    // VIEWMODEL ska inte känna till VIEWS => ingen kod för att t ex öppna eller stänga fönster här 
    // VIEWMODEL SOM SKA HANTERA INLOGGNING, INLOGGAD ANVÄNDARE
    // ANROPA USERMANAGER för autentisering och registrering 
    // Ska signalera till appen när användare är inloggad (via event) 
    // Detta kräver ett "neutralt" sätt att förmedla om inloggning lyckas 
    // KOMMANDON OCH METODER - Samarbetar med UserManagerklassen i Managersmappen
    public class UserManagerViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVAT FÄLT 
        private RelayCommand _loginCommand;

        // PUBLIK EGENSKAP
        public RelayCommand LogInCommand
        {
            get
            {
                // villkor
                if (_loginCommand == null) // OM vi inte har någon metod bunden till kommandot 
                {
                    //_logoutCommand = new RelayCommand(execute => AddUser(), canExecute => UserList.Count > 0);
                }
                return _loginCommand;
            }
        }
        // METOD för att kunna visa inloggningsstatus
        public bool IsAuthenticated => CurrentUser != null;

        // Metod för att lägga till användare i lista 
        private void CreateDefaultUsers()
        {
            _userlist.Add(new User { Username = "LindaMaria", DisplayName = "Administratör", Role = "admin", Password = "0000" });
            //_userlist.Add(new User{Username = "Elsa", DisplayName = "Elsa", Role = "member", Password = "0001" });
            //_userlist.Add(new User{Username = "Elvis", DisplayName = "Elvis", Role = "member", Password = "0002" });
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
