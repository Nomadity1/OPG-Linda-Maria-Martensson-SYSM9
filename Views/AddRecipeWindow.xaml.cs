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
    /// Interaction logic for AddRecipeWindow.xaml
    /// </summary>
    public partial class AddRecipeWindow : Window
    {
        public AddRecipeWindow() //Anger tillhörande ViewModel med objekt som parameter
        {
            InitializeComponent();
            // Instansierar och upprättar samarbete med UserManager och RecipeManager från global variabel i app-resurser
            var userManager = (UserManager)Application.Current.Resources["UserManager"];
            var recipeManager = (RecipeManager)Application.Current.Resources["RecipeManager"];
            // Instansierar tillhörande ViewModel med objektet addRecipeVM
            var addRecipeVM = new AddRecipeViewModel(); 
            // ...och anger objektet som datakontext
            DataContext = addRecipeVM;
            addRecipeVM.SaveSuccess += (s, e) =>
            {
                DialogResult = true; // Meddelar framgång 
                this.Close(); // ...och stänger detta fönster
            };
            // Påminner programmet om vilken datakontexten är
            DataContext = addRecipeVM;
        }
    }
}