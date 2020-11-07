using SwitchController;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    class CaptureCalyrex : IAction
    {
        public CaptureCalyrex()
        {
            Arguments = new ActionArgument[]
            {
            };
        }

        public string Name => "バドレックス厳選";

        public string Description => @"バドレックスを捕まえて個体値を確認する画面を表示する";

        public ActionArgument[] Arguments { get; }

        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            await c.PushButtonAsync(ctx, Button.Home, 1000);
            await c.PushButtonAsync(ctx, Button.X, 1000);
            await c.PushButtonNAsync(ctx, Button.A, 1000, 38);
            await ControllerUtil.DelayAsync(ctx, 30000);

            // 戦闘開始
            await c.PushButtonAsync(ctx, Button.X, 1000);
            await c.PushHatNAsync(ctx, HatState.Right, 150, 3);
            await c.PushButtonAsync(ctx, Button.A, 1000); // マスターボール
            await c.PushButtonNAsync(ctx, Button.A, 1000, 58);

            // キヅナのタヅナ使用
            await c.PushButtonAsync(ctx, Button.X, 1000);
            await c.PushHatNAsync(ctx, HatState.Right, 150, 2);
            await c.PushButtonAsync(ctx, Button.A, 2000);
            await c.PushHatNAsync(ctx, HatState.Left, 150, 1);
            await c.PushHatNAsync(ctx, HatState.Up, 150, 1);
            await c.PushButtonNAsync(ctx, Button.A, 300, 2);
            await c.PushHatNAsync(ctx, HatState.Up, 150, 1);
            await c.PushButtonNAsync(ctx, Button.A, 200, 1);
            await c.PushButtonNAsync(ctx, Button.B, 1000, 15);

            // 確認
            await c.PushButtonAsync(ctx, Button.X, 1000);
            await c.PushHatNAsync(ctx, HatState.Left, 150, 1);
            await c.PushButtonAsync(ctx, Button.A, 2000);
            await c.PushButtonAsync(ctx, Button.R, 2000);
            await c.PushHatNAsync(ctx, HatState.Left, 150, 1);
            await c.PushHatNAsync(ctx, HatState.Down, 150, 2);
        }
    }
}
