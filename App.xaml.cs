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
            // Låt inte programmet stängas automatiskt när MainWindow stängs
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose; // HAR ÄNDRAT SISTA TERMEN  
            //Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown; // LÅTER DENNA LIGGA KVAR SOM REFERENS
        }
    }
}