using CookMaster.Managers;
using CookMaster.ViewModels;
using CookMaster.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 1. Vill visa inloggad användare i hela projektet
            // Instansierar UserManager genom global UserManager i app-resurser 
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansierar Main-viewmodel med objektet userManager som parameter 
            // och anger detta som datakontext 
            DataContext = new MainWindowViewModel(userManager);

            // 2. Vill gömma Main-fönstret medan LogIn-fönstret visas - Gör vi detta någon annan stans? 
            //this.Hide();
            // Instansierar och visar login-fönstret genom objektet loginWindow
            //LogInWindow loginWindow = new LogInWindow();
            //loginWindow.Show();
        }
    }
}