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
        // Klassen RelayCommand tar emot en parameter
        // Parametern execute är en metod (Action) 
        // Parametern canExecute är en funktion (Func) som returnerar en bool (true el false)

        //PRIVAT FÄLT för referenser till metoder som definierar kommandon och vad som ska göras (=Execute)
        private Action<object> execute;

        //FUNKTION som kontrollerar om kommando kan köras (=canExecute) 
        private Func<object, bool>? canExecute;

        //EVENT som signalerar när kommandots möjlighet att köras har ändrats
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
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
        }
        //METOD som kör den logik som tilldelats via execute-metoden
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}