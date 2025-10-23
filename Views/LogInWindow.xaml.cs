using CookMaster.Managers;
using CookMaster.Views;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookMaster.Views
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();

            // 1. Ta emot inloggningsuppgifter 
            // Instansierar och upprättar samarbete med UserManager, från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansierar User Manager ViewModel med objektet userManager
            var userManagerVW = new UserManagerViewModel(userManager); 
            // ...och anger userManagerViewModel-objektet som datakontext
            DataContext = userManagerVW;
            // Anropar LogInSuccess-eventet i UserManagerViewModel
            // som tilldelar objektet det utfall som aktiveras 
            // s = sender (i det här fallet objektet userManagerVM)
            // e = eventets data (det som händer i klassen)
            // += betyder att vi prenumererar på ett event (t ex kopplar en metod till ett event,
            // som körs varje gång eventet triggas)
            userManagerVW.LogInSuccess += (s, e) =>
            {
                DialogResult = true; // meddelar framgång 
                Close(); // Stänger login-fönster
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = userManagerVW;
        }
        // METOD för att ta emot lösenord från passwordbox i login-vyn
        private void Pwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserManagerViewModel userManagerVW)
                userManagerVW.Password = PassWord.Password;
        }
    }
}
