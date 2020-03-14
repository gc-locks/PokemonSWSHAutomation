using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace PokemonAutomation.Action
{
    class SampleAction : IAction
    {
        ActionArgument[] arguments;

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

        public void Call()
        {
            Debug.Write("sample: " + String.Join(",", arguments.Select(a => a.Value)));
        }
    }
}
