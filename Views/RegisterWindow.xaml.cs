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

            // Instansierar och upprättar samarbete med UserManager, från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansierar och upprättar samarbete med RecipeManager, från global variabel i app-resurser
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];

            // Instansierar register-ViewModel med objektet registerVM
            var registerVW = new RegisterViewModel();
            // ...och anger objektet som datakontext
            DataContext = registerVW;
            // Anropar RegistrationSuccess-eventet i UserManagerViewModel
            registerVW.RegisterSuccess += (s, e) =>
            {
				DialogResult = true; // Meddelar framgång 
				this.Close(); // Stänger registreringsfönstret
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = registerVW;
        }

        // METOD för att ta emot lösenord från passwordbox i registrerings-vyn
        // Övriga inmatningar är bundna direkt via DataContext och behöver inte inläsningsmetoder här
        private void NewPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel registerVW)
            {
                registerVW.NewPassword = NewPassWord.Password;
            }
        }
    }
}
