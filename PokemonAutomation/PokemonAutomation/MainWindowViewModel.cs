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
        private string ipAddr;
        private bool controllerConnected;

        public string[] ComPorts
        {
            get => comPorts;
            set
            {
                comPorts = value;
                OnPropertyChanged("ComPorts");
            }
        }

        public IAction[] Actions
        {
            get => actions;
            set
            {
                actions = value;
                OnPropertyChanged("Actions");
            }
        }

        public IAction CurerntAction
        {
            get => currentAction;
            set
            {
                currentAction = value;
                OnPropertyChanged("CurrentAction");
                OnPropertyChanged("ActionRunning");
                OnPropertyChanged("ActionNotRunning");
            }
        }
        public bool ActionRunning => currentAction != null;
        public bool ActionNotRunning => currentAction == null;

        public string IPAddr
        {
            get => ipAddr;
            set
            {
                ipAddr = value;
                OnPropertyChanged("IPAddr");
            }
        }

        public bool ControllerConnected
        {
            get => controllerConnected;
            set
            {
                controllerConnected = value;
                OnPropertyChanged("ControllerConnected");
                OnPropertyChanged("ControllerDisconnected");
            }
        }
        public bool ControllerDisconnected => !controllerConnected;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
