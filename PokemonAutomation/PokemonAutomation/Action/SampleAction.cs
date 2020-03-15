using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using SwitchController;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    class SampleAction : IAction
    {
        readonly ActionArgument[] arguments;

        public SampleAction()
        {
            arguments = new ActionArgument[]
            {
                new ActionArgument("arg1")
            };
        }

        public string Name => "sample";

        public string Description => "sample description";

        public ActionArgument[] Arguments
        {
            get { return arguments; }
        }

        public async Task CallAsync(CancellationToken ctx, IController controller)
        {
            Debug.Write("sample: " + string.Join(",", arguments.Select(a => a.Value)));
            await ControllerUtil.DelayAsync(ctx, 100);
        }
    }
}
