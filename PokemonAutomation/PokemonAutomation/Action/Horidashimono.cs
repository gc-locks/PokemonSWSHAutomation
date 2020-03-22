using SwitchController;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    public class Horidashimono : IAction
    {
        public Horidashimono()
        {
            Arguments = new ActionArgument[] { };
        }
        public ActionArgument[] Arguments { get; }

        public string Name => "ほりだしもの";

        public string Description => @"ほりだしものを買う
初期条件は以下の通り
1.ランクマッチバグを使用し、使える状態であること
2.ラテラルタウンのほりだしもの市の前であること
";


        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            while (!ctx.IsCancellationRequested)
            {
                await c.PushButtonNAsync(ctx, Button.A, 100, 20);
                await c.PushButtonNAsync(ctx, Button.B, 100, 40);

                await Common.Forward1Day(ctx, c, true);

                await ControllerUtil.DelayAsync(ctx, 1000);
            }
        }
    }
}
