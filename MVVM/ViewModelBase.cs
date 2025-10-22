using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.MVVM
{
    // BASKLASS FÖR VIEWMODELS FÖR ATT IMPLEMENTERA INOTIFY... 
    class ViewModelBase : INotifyPropertyChanged
    {
        // Implementera INotifyPropertyChanged genom generell händelse 
        public event PropertyChangedEventHandler? PropertyChanged;
        // ...och metod som kan användas av alla klasser
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
