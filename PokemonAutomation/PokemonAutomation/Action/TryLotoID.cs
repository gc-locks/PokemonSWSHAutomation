using SwitchController;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    public class TryLotoID : IAction
    {
        public TryLotoID()
        {
            Arguments = new ActionArgument[] { };
        }
        public ActionArgument[] Arguments { get; }

        public string Name => "ロトミ IDくじ";

        public string Description => @"IDくじを引き続ける from https://github.com/interimadd/NintendoSwitchControll

初期条件は以下の通り
1.ランクマッチバグを使用し、使える状態であること
2.31日まである月の1日からスタートすること
3.ポケセンのロトミの前であること
";


        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            while (!ctx.IsCancellationRequested)
            {
                // ロトミ起動 > IDくじ
                await c.PushButtonNAsync(ctx, Button.A, 500, 2);
                await ControllerUtil.DelayAsync(ctx, 500);
                await c.PushHatAsync(ctx, HatState.Down, 150);
                await c.PushButtonNAsync(ctx, Button.A, 100, 20);
                await c.PushButtonNAsync(ctx, Button.B, 100, 80);

                await Common.Forward1Day(ctx, c, true);

                await ControllerUtil.DelayAsync(ctx, 1000);
            }
        }
    }
}
