using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schlechtums.Core.BaseClasses
{
    /// <summary>
    /// Raised before a property is changed and provides the old value and new value;
    /// </summary>
    public interface INotifyBeforePropertyChanged
    {
        event BeforePropertyChangedHandler BeforePropertyChanged;
    }

    public delegate void BeforePropertyChangedHandler(Object sender, BeforePropertyChangedEventArgs e);

    public class BeforePropertyChangedEventArgs
    {
        public BeforePropertyChangedEventArgs(string propertyName, Object oldValue, Object newValue)
        {
            this.PropertyNames = new List<string>(1) { propertyName };
            this.OldValues = new List<Object>(1) { oldValue };
            this.NewValues = new List<Object>(1) { newValue };
        }

        public BeforePropertyChangedEventArgs(List<string> propertyNames, List<Object> oldValues, List<Object> newValues)
        {
            if (propertyNames.Count != oldValues.Count || propertyNames.Count != newValues.Count)
                throw new Exception("Property names count does not match old values count and new values count");

            this.PropertyNames = propertyNames;
            this.OldValues = oldValues;
            this.NewValues = newValues;
        }

        public List<string> PropertyNames { get; private set; }
        public List<Object> OldValues { get; private set; }
        public List<Object> NewValues { get; private set; }
    }
}