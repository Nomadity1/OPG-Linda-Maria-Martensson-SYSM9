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
    //UPPGIFTER: visa inloggad användare, visa receptlista, sök- och filtreringsfunktion
    public class MainWindowViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // PUBLIK EGENSKAP som använder sig av UserManager-klassen (skapar förutsättning för samarbete med UserManager)
        public UserManager UserManager { get;  }
        
        // PUBLIK EGENSKAP i form av kommando för utloggning (indikerar på användning av basklass RelayCommand)
        public ICommand LogOutCommand { get; } // Bunden med knapp i MainWindow-vyn

        // KONSTRUKTOR med villkorssats för att visa login-fönster och användare 
        public MainWindowViewModel(UserManager userManager) // Upprättar samarbete mln Main och UserManager
        {
            UserManager = userManager;
            // Visa loginfönster när ingen är inloggad
            if (!UserManager.IsAuthenticated)
            {
                // HASSANS KOD: Anropar metod för att visa LogIn-vyn
                ShowLogin(); 
                //LogInWindow loginWindow = new LogInWindow();
                //loginWindow.Show();
                // Logout-kommandot kan endast och när som helst köras när någon är inloggad
                LogOutCommand = new RelayCommand(
                execute => UserManager.Logout(),
                canExecute => UserManager.IsAuthenticated);
            }
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
            var login = new LogInWindow();
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
        }
        // METOD för avv visa inloggnignsfönstret
        private void ShowLogin()
        {
            var login = new LogInWindow();
            var result = login.ShowDialog();
            // Stänger om inloggning avbryts
            if (result != true)
                Application.Current.Shutdown(); 
        }

        // Generellt EVENT och generell METOD för att möjliggöra binding 
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
