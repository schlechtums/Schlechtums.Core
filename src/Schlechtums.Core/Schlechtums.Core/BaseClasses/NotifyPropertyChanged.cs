using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlechtums.Core.BaseClasses
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged, INotifyBeforePropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual bool RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));

                return true;
            }

            return false;
        }

        public event BeforePropertyChangedHandler BeforePropertyChanged;
        protected virtual bool RaiseBeforePropertyChanged(string propertyName, Object oldValue, Object newValue)
        {
            var handler = this.BeforePropertyChanged;
            if (handler != null)
            {
                handler(this, new BeforePropertyChangedEventArgs(propertyName, oldValue, newValue));
                return true;
            }

            return false;
        }

        protected virtual bool RaiseBeforePropertyChanged(List<string> propertyNames, List<Object> oldValues, List<Object> newValues)
        {
            var handler = this.BeforePropertyChanged;
            if (handler != null)
            {
                handler(this, new BeforePropertyChangedEventArgs(propertyNames, oldValues, newValues));
                return true;
            }

            return false;
        }
    }
}