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
        public UserDetailsWindow()
        {
            InitializeComponent();
            // Instansierar och upprättar samarbete med User- och RecipeManager från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];

            // Instansierar och upprättar samarbete med tillhörande ViewModel
            var userDetailsVW = new UserDetailsViewModel(userManager);
            // ...och anger objektet som datakontext
            DataContext = userDetailsVW;
            // Anropar UpdateSuccess-eventet i userDetailsViewModel
            userDetailsVW.UpdateSuccess += (s, e) =>
            {
                DialogResult = true; // Meddelar framgång 
                this.Close(); // Stänger detta fönster
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = userDetailsVW;
        }

        // Metod för att ta emot nytt lösenord
        private void UpdatedPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsViewModel userDetailsVW)
            {
                userDetailsVW.UpdatedPassword = UpdatedPassWord.Password;
            }
        }

        private void UpdatedRepeatedPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsViewModel userDetailsVW)
            {
                userDetailsVW.UpdatedRepeatedPassword = UpdatedRepeatedPassWord.Password;
            }
        }
    }
}
