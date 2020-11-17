using SwitchController;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    class Tournament : IAction
    {
        public Tournament()
        {
            Arguments = new ActionArgument[]
            {
                new ActionArgument("Bボタンに対するAボタンの回数")
                {
                    Value = "10"
                }
            };
        }

        public string Name => "トーナメント周回";

        public string Description => @"
条件
1. トーナメントの通路にいること
2. Aボタン連打で相手を倒せること
";

        public ActionArgument[] Arguments { get; }

        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            if (!int.TryParse(Arguments[0].Value, out int aButtons))
            {
                throw new Exception(string.Format("invalid format: {0} must be integer", Arguments[0].Name));
            }

            c.TiltStick(ctx, Stick.LSTICK, 0.25f, -1.0f);

            while (!ctx.IsCancellationRequested)
            {
                await c.PushButtonNAsync(ctx, Button.A, 200, aButtons);
                await c.PushButtonNAsync(ctx, Button.B, 200, 1);
            }

            c.TiltStick(ctx, Stick.LSTICK, 0.0f, 0.0f);
        }
    }
}
