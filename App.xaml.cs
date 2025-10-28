using CookMaster.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CookMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Förhindra att programmet (appen) stängs när MainWindow stängs (anligt Hassans tips)
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Fick tips av chatGPT att lägga ShowLogin här! 
            //ShowLogIn();
            // Låt inte programmet stängas automatiskt när MainWindow stängs
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // METOD för avv visa inloggnignsfönstret
        }
        //private void ShowLogin()
        //{
        //    var login = new LogInWindow();
        //    var result = login.ShowDialog();
        //    // Stänger om inloggning avbryts
        //    if (result != true)
        //        Application.Current.Shutdown();
        //}
    }
}