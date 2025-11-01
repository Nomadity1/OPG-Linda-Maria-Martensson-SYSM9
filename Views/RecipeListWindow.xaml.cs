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
        private RecipeListViewModel recipeListVM;

        // UPPGIFTER: Visa receptlista, hantera recept (lägg till, redigera, ta bort), sökfunktion, filtreringsfunktion?

        public RecipeListWindow()
        {
            InitializeComponent();
            //// Instansierar och upprättar samarbete med UserManager, från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            // Instansierar och upprättar samarbete med RecipeManager, från global variabel i app-resurser
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];

            // Instansierar tillhörande ViewModel med objektet recipeListVM
            var recipeListVW = new RecipeListViewModel();
            // ...och anger objektet som datakontext
            DataContext = recipeListVW;
        }

        public RecipeListWindow(RecipeListViewModel recipeListVM)
        {
            this.recipeListVM = recipeListVM;
        }
    }
}
