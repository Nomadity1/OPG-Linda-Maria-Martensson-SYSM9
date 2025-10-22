using CookMaster.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.ViewModels
{
    // VIEWMODEL SOM SKA HANTERA INLOGGAD ANVÄNDARE
    // KOMMANDON OCH METODER - Samarbetar med UserManagerklassen i Managersmappen
    public class UserManagerViewModel
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
    }
}
