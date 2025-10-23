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
            // Instansierar UserManager igen, från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansiera login-viewmodellen 
            // Skapa en ny instans av LoginViewModel och sätt den som DataContext
            var = new UserManagerViewModel(userManager);
            DataContext = vm;
            // 2. Om inloggning genomförs - Stäng Log in (och säkerställ att Main visas igen)

        }
        // Metod för att hantera lösenord 
        private void PassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserManagerViewModel vm)
                vm.Password = Password.Password;
        }
    }
}
