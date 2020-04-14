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
                new FastForwardDays(),
                new Inqubate(),
                new TryLotoID(),
                new Horidashimono(),
                new RaidBattle(),
                new GainWatt(),
            };

            InitializeComponent();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (controller != null)
                controller.Dispose();
        }

        private bool Running => vm.CurerntAction != null;
        private bool ControllerAvailable => controller != null && controller.Available();

        private void ConnectEthernetController(object sender, RoutedEventArgs e)
        {
            controller = new EthernetConnector(vm.IPAddr);
            vm.ControllerConnected = true;
        }

        private void DisconnectEthernetController(object sender, RoutedEventArgs e)
        {
            controller.Dispose();
            controller = null;
            vm.ControllerConnected = false;
        }

        private void InputButton(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)d;
            if (!Enum.TryParse(button.Name, out SwitchController.Button b))
            {
                return;
            }
            if (ControllerAvailable)
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
            if (ControllerAvailable)
            {
                Debug.Print("{0}: {1}", b.ToString(), e.NewValue);
                controller.InputHat((bool)e.NewValue ? b : HatState.Center);
            }
        }

        private void CallAction(object sender, RoutedEventArgs e)
        {
            if (Running || !ControllerAvailable)
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
            if (Running || !ControllerAvailable)
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
