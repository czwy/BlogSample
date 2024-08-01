using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SelectorDragDropSamples
{
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanging(string propertyName)
        {
            var handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        protected void Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(oldValue, newValue))
            {
                oldValue = newValue;
                RaisePropertyChanged(propertyName);
            }
        }

    }
}
