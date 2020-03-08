using SwitchController;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokemonAutomation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;
        private Connector controller;

        public MainWindow()
        {
            vm = new MainWindowViewModel();
            DataContext = vm;
            InitializeComponent();
            GetSerialPorts();
        }

        private void GetSerialPorts()
        {
            vm.ComPorts = SwitchController.Connector.GetSerialPorts();
        }

        private void COMPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (controller != null)
            {
                controller.Dispose();
            }

            var cb = (System.Windows.Controls.ComboBox)sender;
            controller = new Connector((string)cb.SelectedItem);
        }

        private void InputButton(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)d;
            if (!Enum.TryParse(button.Name, out SwitchController.Button b))
            {
                return;
            }
            if (controller != null && controller.Available())
            {
                Debug.Print("{0}: {1}", b.ToString(), e.NewValue);
                controller.InputButton(b, (bool)e.NewValue ? ButtonState.PRESS : ButtonState.RELEASE);
            }
        }
    }
}
