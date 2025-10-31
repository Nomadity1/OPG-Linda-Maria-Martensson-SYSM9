using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.MVVM
{
    // BASKLASS FÖR VIEWMODELS FÖR ATT IMPLEMENTERA INOTIFYPROPERTYCHANGED 
    class ViewModelBase : INotifyPropertyChanged
    {
        // Implementera INotifyPropertyChanged genom generellt EVENT 
        public event PropertyChangedEventHandler? PropertyChanged;
        // ...och och generell METOD 
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
