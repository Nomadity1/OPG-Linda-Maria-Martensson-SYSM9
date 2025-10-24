using CookMaster.Managers;
using CookMaster.Views;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CookMaster.ViewModels
{
    //HUVUDSAKLIG VIEWMODEL för HUVUDFÖNSTRET (MainWindow)
    //UPPGIFTER: visa inloggad användare
    public class MainWindowViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PUBLIK EGENSKAP som använder sig av UserManager-klassen (skapar förutsättning för samarbete med UserManager)
        public UserManager UserManager { get;  }
        
        // PUBLIK EGENSKAP i form av kommando för utloggning (indikerar på användning av basklass RelayCommand)
        public ICommand LogOutCommand { get; }

        // KONSTRUKTOR med villkorssats för att visa login-fönster och användare 
        public MainWindowViewModel(UserManager userManager) // Upprättar samarbete mln Main och UserManager
        {
            UserManager = userManager;
            // Visa loginfönster när ingen är inloggad
            if (!UserManager.IsAuthenticated)
            {
                LogInWindow loginWindow = new LogInWindow();
                loginWindow.Show();
                // Logout-kommandot kan endast och när som helst köras när någon är inloggad
                var LogoutCommand = new RelayCommand(
                execute => UserManager.Logout(),
                canExecute => UserManager.IsAuthenticated);
            }
        }
        private void LogOut()
        {
            UserManager.Logout();
            // Sparar referens till gamla MainWindow
            var oldMain = Application.Current.MainWindow; 
            // Stänger MainWindow utan avv programmet avslutas
            if (oldMain != null)
            {
                oldMain.Close(); 
            }
            // Visa LogIn-fönstret
            var login = new LogInWindow();
            var result = login.ShowDialog(); 
            // Om användarens loggar in igen
            if (result == true)
            {
                var newMain = new MainWindow();
                Application.Current.MainWindow = newMain;
                newMain.Show(); 
            }
            else
            {
                // Avsluta programmet om login avbryts
                Application.Current.Shutdown(); 
            }
        }
        // METOD för avv visa inloggnignsfönstret
        private void ShowLogin()
        {
            var login = new LogInWindow();
            var result = login.ShowDialog();
            if (result != true)
                Application.Current.Shutdown(); 
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
