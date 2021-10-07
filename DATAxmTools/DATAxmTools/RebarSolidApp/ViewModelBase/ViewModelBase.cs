using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAxmTools.RebarSolidApp.ViewModelBase
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;

            if (eventHandler == null)
                return;
           
            eventHandler((object)this, new PropertyChangedEventArgs(propertyName));

        }
    }


}
