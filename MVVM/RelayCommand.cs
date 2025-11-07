using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookMaster.MVVM
{
    public class RelayCommand : ICommand
    {
        // BASKLASS FÖR ATT DEFINIERA KOMMANDON - KOD KOMMER FRÅN HASSANS FIL PÅ GITHUB 

        // PRIVAT DELEGAT som pekar på en metod, som i sin tur tar ett objekt som parameter
        // och inte returnerar något. Körs när kommandot exekveras (t ex vid användares aktivitet). 
        // "referenser till metoder som definierar kommandon och vad som ska göras" 
        private Action<object> execute;

        // PRIVAT DELEGAT som pekar på en metod, som i sin tur tar ett objekt som parameter och returnerar
        // en bool (true el. false). Används för att avgöra om kommandot kan köras eller inte.
        private Func<object, bool>? canExecute;

        // EVENT som tillhör ICommand-gränssnittet. Meddelar UI't när CanExecute-villkoret ändras
        // så att en knappstatus uppdateras (aktiveras resp. inaktiveras).
        public event EventHandler? CanExecuteChanged 
        {
            // Här kopplas eventet till den globala CommandManager som hanterar kommandon i WPF-applikationen
            // Anropet sker automatiskt vid förändringar som kan påverka CanExecute-villkoret
            // håller alltså reda på när knappar ska aktiveras/inaktiveras åt mig! :-) 
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
                // - execute körs när man klickar på knappen
                // - canExecute bestämmer om knappen ska vara aktiv
                // - CanExecuteChanged håller UI:t uppdaterat automatiskt
        }

        // KONSTRUKTOR 
        public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        //METOD som kontrollerar om kommandot kan köras eller inte
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
            // Här är det viktigt att ange att canExecute kan ta ett null-värde
            // annars är risken stor att programmet kraschar vid körning 
        }

        //METOD som kör den logik som tilldelats via execute-metoden
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}