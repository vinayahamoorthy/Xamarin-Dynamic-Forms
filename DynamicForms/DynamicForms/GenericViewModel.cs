using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForms
{
    public class GenericViewModel : INotifyPropertyChanged
    {
        private IDictionary<string, object> _list;
        public event PropertyChangedEventHandler PropertyChanged;
        public IDictionary<string, object> FormData
        {
            get 
            { 
                return _list; 
            }
            set
            {
                _list = value;
            }
        }

        public GenericViewModel()
        {
            PropertyChanged += NotifyPropertyChanged;
        }


        protected void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(e.PropertyName));
            }
        }


    }
}
