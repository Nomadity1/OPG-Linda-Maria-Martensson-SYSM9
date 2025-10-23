using CookMaster.Managers;
using CookMaster.Views;
using CookMaster.Models;
using CookMaster.MVVM;
using CookMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.ViewModels
{
    //HUVUDSAKLIG VIEWMODEL för HUVUDFÖNSTRET (MainWindow)
    //UPPGIFTER: visa inloggad användare
    public class MainWindowViewModel : INotifyPropertyChanged // Implementerar interface för att möjliggöra "data binding"
    {
        // ATTRIBUT: PUBLIKT FÄLT som använder sig av UserManager-klassen 
        public UserManager UserManager { get;  }
        // ATTRIBUT: PUBLIK EGENSKAP i form av kommando för utloggning 
        public ICommand LogOutCommand { get; }

        // KONSTRUKTOR: ska lägga till villkorssats för autentisiering av användare 
        public MainWindowViewModel(UserManager userManager)
        {
            UserManager = userManager;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
