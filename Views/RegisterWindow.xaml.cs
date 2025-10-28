using CookMaster.Managers;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            // Ta emot registreringsuppgifter 
            // Instansierar och upprättar samarbete med UserManager, från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansierar User Manager ViewModel med objektet userManager
            var userManagerVW = new UserManagerViewModel(userManager);
            // ...och anger userManagerViewModel-objektet som datakontext
            DataContext = userManagerVW;
            // Anropar RegistrationSuccess-eventet i UserManagerViewModel
            // ...som tilldelar objektet det utfall som aktiveras 
            // s = sender (i det här fallet objektet userManagerVM)
            // e = eventets data (det som händer i klassen)
            // += betyder att vi prenumererar på ett event (t ex kopplar en metod till ett event,
            // som körs varje gång eventet triggas)
            userManagerVW.RegistrationSuccess += (s, e) =>
            {
                DialogResult = true; // meddelar framgång 
                var loginWindow = new LogInWindow(); // Instansierar loginfönster
                loginWindow.Show(); // Visar loginfönster
                this.Close(); // Stänger registreringsfönstret
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = userManagerVW;
        }

        // METODER för att ta emot lösenord från passwordboxar i registrerings-fönstret
        // Övriga inmatningar är bundna direkt via DataContext och behöver inte inläsningsmetoder här
        private void NewPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserManagerViewModel userManagerVW)
            {
                // Egenskapen Password i UserManagerViewModel nås genom objektet userManagerVM
                // ...och tilldelas inmatat värde från LogIn-fönstrets password-box "PassWord"
                userManagerVW.Password = NewPassWord.Password;
            }
        }
        private void RepeatPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserManagerViewModel userManagerVW)
            {
                // Egenskapen NewPassword i UserManagerViewModel nås genom objektet userManagerVM
                // ...och tilldelas inmatat värde från LogIn-fönstrets password-box "PassWord"
                userManagerVW.Password = NewPassWord.Password;
            }
        }
    }
}
