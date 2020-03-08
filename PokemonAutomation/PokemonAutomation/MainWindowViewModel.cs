using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PokemonAutomation
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        private string[] comPorts;

        public string[] ComPorts
        {
            get
            {
                return comPorts;
            }
            set
            {
                comPorts = value;
                OnPropertyChanged("ComPorts");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
