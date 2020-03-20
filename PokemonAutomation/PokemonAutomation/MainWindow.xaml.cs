using PokemonAutomation.Action;
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
using System.Threading;

namespace PokemonAutomation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EthernetConnector controller;
        private readonly MainWindowViewModel vm;
        private CancellationTokenSource cs;

        public MainWindow()
        {
            vm = new MainWindowViewModel();
            DataContext = vm;

            vm.Actions = new IAction[]
            {
                new ForwardDays(),
                new Inqubate(),
            };

            InitializeComponent();

            controller = new EthernetConnector("192.168.1.177");
        }

        private bool Running => vm.CurerntAction != null;

        private void GetSerialPorts()
        {
            //vm.ComPorts = SwitchController.Connector.GetSerialPorts();
        }

        private void COMPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (controller != null)
            //{
            //    controller.Dispose();
            //}

            //var cb = (System.Windows.Controls.ComboBox)sender;
            //controller = new Connector((string)cb.SelectedItem);
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

        private void InputHat(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)d;
            if (!Enum.TryParse(button.Name, out SwitchController.HatState b))
            {
                return;
            }
            if (controller != null && controller.Available())
            {
                Debug.Print("{0}: {1}", b.ToString(), e.NewValue);
                controller.InputHat((bool)e.NewValue ? b : HatState.Center);
            }
        }

        private void CallAction(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                return;
            }

            var action = (IAction)ActionSelector.SelectedItem;

            cs = new CancellationTokenSource();
            vm.CurerntAction = action;

            Task.Run(async () =>
            {
                await action.CallAsync(cs.Token, controller);
                ClearAction();
            });
        }

        private async void StopAction(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                cs?.Cancel();
            }
            ClearAction();
            await controller.ClearAsync();
        }

        private void ClearAction()
        {
            if (cs != null)
            {
                cs.Dispose();
            }
            cs = null;
            vm.CurerntAction = null;
        }
    }
}
