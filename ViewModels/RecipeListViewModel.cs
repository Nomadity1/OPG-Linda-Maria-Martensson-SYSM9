using CookMaster.Managers;
using CookMaster.MVVM;
using CookMaster.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    public class RecipeListViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PRIVATA FÄLT 
        private readonly UserManager _userManager;
        private string _error;
        //private readonly (???) RecipeManager _recipeManager; SÅ SMÅNINGOM!! 

        // PUBLIK EGENSKAP som använder sig av UserManager-klassen (skapar förutsättning för samarbete med UserManager)
        public UserManager UserManager { get; }
        
        public ICommand? LogOutCommand { get; } // Bunden med knapp i MainWindow-vyn, gör null-able med ? 

        public RecipeListViewModel(UserManager UserManager)
        {
            // Visa loginfönster om ingen är inloggad
            if (!UserManager.IsAuthenticated)
                ShowLogin();
            // UTLOGGNING
        }

        private void ShowLogin()
        {
            var login = new MainWindow();
            var result = login.ShowDialog();

            if (result != true)
                Application.Current.Shutdown();
        }
        private void Logout()
        {
            // Anropar metod i UserManager
            UserManager.Logout();
            // Sparar referens till gamla MainWindow
            var oldMain = Application.Current.MainWindow;
            // Stänger MainWindow utan att programmet avslutas
            if (oldMain != null)
            {
                oldMain.Close();
            }
            // Visa LogIn-fönstret vid utloggning
            var login = new MainWindow();
            var result = login.ShowDialog();
            // Om användarens loggar in igen
            if (result == true)
            {
                // ...så visas Main-vyn 
                var newMain = new MainWindow();
                Application.Current.MainWindow = newMain;
                newMain.Show();
            }
            else
            {
                // ANNARS avslutas programmet (om login t ex avbryts)
                Application.Current.Shutdown();
            }
            //// Logout-kommandot kan endast och när som helst köras när någon är inloggad
            //LogOutCommand = new RelayCommand(execute => UserManager.Logout(), canExecute => UserManager.IsAuthenticated);
        }
        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
