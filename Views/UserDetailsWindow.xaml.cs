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
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        // UPPGIFTER: Visa befintliga uppgifter, ta emot eventuella ändringar, spara (=uppdatera) användaruppgifter
        public UserDetailsWindow()
        {
            InitializeComponent();
            // Instansierar och upprättar samarbete med UserManager, från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansierar userdetails-ViewModel med objektet userDetailsVW 
            var userDetailsVW = new UserDetailsWindow(userManager);
            // ...och anger objektet som datakontext
            DataContext = userDetailsVW;
            // Anropar UpdateSuccess-eventet i userDetailsViewModel
            // ...som tilldelar objektet det utfall som aktiveras 
            // s = sender (i det här fallet objektet userDetailsVW )
            // e = eventets data (det som händer i klassen)
            // += betyder att vi prenumererar på ett event (t ex kopplar en metod till ett event,
            // som körs varje gång eventet triggas)

            userDetailsVW.UpdateSuccess += (s, e) =>
            {
                this.Close(); // Stänger detta fönster
                var mainWindow = new MainWindow(); // Instansierar loginfönster
                mainWindow.Show(); // Visar loginfönster
                DialogResult = true; // Meddelar framgång 
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = userDetailsVW;
        }
        // Metod för att ta emot nytt lösenord
        private void PassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsWindow userDetailsVW)
            {
                // Egenskapen NewPassword i User Details View Model tilldelas
                // inmatat värde från registreringsfönstrets password-box "NewPassWord"
                userDetailsVW.UpdatedPassword = PassWord.Password;
            }
        }
        // Metod för att ta emot upprepatlösenord
        private void RepeatedPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsWindow userDetailsVW)
            {
                // Egenskapen RepeatedPassword i User Details View Model tilldelas
                // inmatat värde från registreringsfönstrets password-box "RepeatPassWord"
                userDetailsVW.RepeatedPassWord = RepeatedPassWord.Password;
        }
    }
}
