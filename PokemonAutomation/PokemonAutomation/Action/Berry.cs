using SwitchController;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    class Berry : IAction
    {
        public Berry()
        {
            Arguments = new ActionArgument[]
            {
                new ActionArgument("A連打する時間[s]")
                {
                    Value = "65"
                },
                new ActionArgument("回数")
                {
                    Value = "400"
                }
            };
        }

        public string Name => "きのみ回収";

        public string Description => @"
条件
1. レートバトルをした後であること
2. Aボタンできのみを回収できること
";

        public ActionArgument[] Arguments { get; }

        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            if (!int.TryParse(Arguments[0].Value, out int duration))
            {
                throw new Exception(string.Format("invalid format: {0} must be integer", Arguments[0].Name));
            }
            if (!int.TryParse(Arguments[1].Value, out int count))
            {
                throw new Exception(string.Format("invalid format: {0} must be integer", Arguments[1].Name));
            }

            for (int i = 0; i < count && !ctx.IsCancellationRequested; i++)
            {
                await Common.ForwardDays(ctx, c, 2);

                await c.PushButtonTAsync(ctx, Button.A, 200, duration * 1000);
                await c.PushButtonNAsync(ctx, Button.B, 500, 5);
            }
        }
    }
}
