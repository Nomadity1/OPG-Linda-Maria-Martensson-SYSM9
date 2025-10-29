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
            // Instansierar register-ViewModel med objektet registerVM
            var registerVW = new RegisterViewModel(userManager);
            // ...och anger objektet som datakontext
            DataContext = registerVW;
            // Anropar RegistrationSuccess-eventet i UserManagerViewModel
            // ...som tilldelar objektet det utfall som aktiveras 
            // s = sender (i det här fallet objektet userManagerVM)
            // e = eventets data (det som händer i klassen)
            // += betyder att vi prenumererar på ett event (t ex kopplar en metod till ett event,
            // som körs varje gång eventet triggas)
            registerVW.RegisterSuccess += (s, e) =>
            {
                DialogResult = true; // meddelar framgång 
                var mainWindow = new MainWindow(); // Instansierar loginfönster
                mainWindow.Show(); // Visar loginfönster
                this.Close(); // Stänger registreringsfönstret
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = registerVW;
        }

        // METODER för att ta emot lösenord från passwordboxar i registrerings-fönstret
        // Övriga inmatningar är bundna direkt via DataContext och behöver inte inläsningsmetoder här
        private void NewPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel registerVW)
            {
                // Egenskapen NewPassword i Register View Model tilldelas
                // inmatat värde från registreringsfönstrets password-box "NewPassWord"
                registerVW.NewPassword = NewPassWord.Password;
            }
        }
        private void RepeatPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel registerVW)
            {
                // Egenskapen RepeatPassword i Register View Model tilldelas
                // inmatat värde från registreringsfönstrets password-box "RepeatPassWord"
                registerVW.RepeatPassword = RepeatPassWord.Password;
            }
        }
    }
}
