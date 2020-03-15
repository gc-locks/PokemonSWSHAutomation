using SwitchController;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation
{
    public interface IAction
    {
        string Name { get; }
        string Description { get; }
        ActionArgument[] Arguments { get; }
        Task CallAsync(CancellationToken ctx, IController controller);
    }

    public class ActionArgument : INotifyPropertyChanged
    {
        private string val;

        public string Name { get; private set; }
        public string Value
        {
            get => val;
            set
            {
                val = value;
                OnPropertyChanged("Value");
            }
        }

        public ActionArgument(string name)
        {
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
