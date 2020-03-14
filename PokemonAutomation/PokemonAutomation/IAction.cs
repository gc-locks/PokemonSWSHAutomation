using SwitchController;
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

    public class ActionArgument
    {
        public string Name { get; private set; }
        public string Value { get; set; }

        public ActionArgument(string name)
        {
            Name = name;
        }
    }
}
