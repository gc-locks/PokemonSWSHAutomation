using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace PokemonAutomation
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        private string[] comPorts;
        private IAction[] actions;
        private IAction currentAction;

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

        public IAction[] Actions
        {
            get
            {
                return actions;
            }
            set
            {
                actions = value;
                OnPropertyChanged("Actions");
            }
        }

        public IAction CurerntAction
        {
            get
            {
                return currentAction;
            }
            set
            {
                currentAction = value;
                OnPropertyChanged("CurrentAction");
                OnPropertyChanged("GetExecuteVisibility");
                OnPropertyChanged("GetStopVisibility");
            }
        }

        public Visibility GetExecuteVisibility
        {
            get
            {
                return currentAction != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility GetStopVisibility
        {
            get
            {
                return currentAction == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
