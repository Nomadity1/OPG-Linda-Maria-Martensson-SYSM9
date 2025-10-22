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
            // Gömmer Main-fönstret medan LogIn-fönstret visats
            this.Hide();
            // Instansierar login-fönstret genom objektet loginWindow
            LogInWindow loginWindow = new LogInWindow();
            // ...och visar det
            loginWindow.Show();
        }
    }
}