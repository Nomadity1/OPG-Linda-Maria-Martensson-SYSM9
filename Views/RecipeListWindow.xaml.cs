using CookMaster.Managers;
using CookMaster.Models;
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
    /// Interaction logic for RecipeListWindow.xaml
    /// </summary>
    public partial class RecipeListWindow : Window
    {
        // UPPGIFTER: Visa receptlista, hantera recept (lägg till, redigera, ta bort), sökfunktion, filtreringsfunktion?

        // PRIVAT FÄLT för instansiering längre ner 
        private MainViewModel? _mainViewModel;
        public RecipeListWindow()
        {
            InitializeComponent();

            //// Instansierar och upprättar samarbete med UserManager, från global variabel i app-resurser
            //var userManager = (UserManager)Application.Current.Resources["UserManager"];
            //// Instansierar register-ViewModel med objektet registerVM
            //var recipeListVW = new RecipeListViewModel(user, _recipeManager, _userManager);
            //// ...och anger objektet som datakontext
            //DataContext = recipeListVW;
        }
    }
}
