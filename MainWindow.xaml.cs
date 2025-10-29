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
        // UPPGIFTER: Ta emot inloggningsuppgifter, ta emot knapptryckningar (log in, register, forgot password) 

        // PRIVAT FÄLT för instansiering längre ner 
        private MainViewModel _mainViewModel;
        public MainWindow()
        {
            InitializeComponent();

            // Instansierar och upprättar samarbete med UserManager, från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansierar register-ViewModel med objektet registerVM
            var mainVW = new MainViewModel();
            // ...och anger objektet som datakontext
            DataContext = mainVW;
            // Anropar LogInSuccess-eventet i MainViewModel
            // ...som tilldelar objektet det utfall som aktiveras 
            // s = sender (i det här fallet objektet mainVM)
            // e = eventets data (det som händer i klassen)
            // += betyder att vi prenumererar på ett event (t ex kopplar en metod till ett event,
            // som körs varje gång eventet triggas)
            mainVW.LogInSuccess += (s, e) =>
            {
                DialogResult = true; // Meddelar framgång 
                Close(); // ...och stänger detta fönster
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = mainVW;
        }
        // METOD för att ta emot lösenord från passwordbox (för att undvika bindning som hör hemma i en senare del av utbildningen) 
        // UserName är bundet direkt via DataContext (UserManagerViewModel) och behöver inte en inläsningsmetod här
        private void PassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Egenskapen Password i UserManagerViewModel nås genom objektet userManagerVM
            if (DataContext is MainViewModel mainVW)
                // ...och tilldelas inmatat värde från LogIn-fönstrets password-box "PassWord"
                mainVW.Password = PassWord.Password;
        }
    }
}