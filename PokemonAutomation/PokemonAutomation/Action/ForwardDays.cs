using SwitchController;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    class ForwardDays : IAction
    {
        public ForwardDays()
        {
            Arguments = new ActionArgument[]
            {
                new ActionArgument("現在の日付")
                {
                    Value = DateTime.Now.ToString("yyyy-MM-dd")
                },
                new ActionArgument("進める日数")
                {
                    Value = "1"
                },
                new ActionArgument("リセット")
                {
                    Value = false.ToString()
                },
            };
        }

        public string Name => "N日進める";

        public string Description => @"巣穴を用いてN日時計をすすめる from https://github.com/interimadd/NintendoSwitchControll";

        public ActionArgument[] Arguments { get; }

        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            if (!DateTime.TryParseExact(Arguments[0].Value, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime clock))
            {
                throw new Exception(string.Format("invalid format: {0} must be yyyy-MM-dd", Arguments[0].Name));
            }
            if (!int.TryParse(Arguments[1].Value, out int days))
            {
                throw new Exception(string.Format("invalid format: {0} must be integer", Arguments[1].Name));
            }
            if (!bool.TryParse(Arguments[2].Value, out bool reset))
            {
                throw new Exception(string.Format("invalid format: {0} must be boolean", Arguments[2].Name));
            }

            if (reset)
            {
                await c.PushButtonAsync(ctx, Button.Home, 1000);
                await c.PushButtonAsync(ctx, Button.X, 1000);
                await c.PushButtonNAsync(ctx, Button.A, 1000, 35);
            }

            await c.PushButtonNAsync(ctx, Button.B, 1000, 4);

            if (reset)
            {
                await ControllerUtil.DelayAsync(ctx, 1000);
                await c.PushButtonNAsync(ctx, Button.A, 1000, 2);
                await c.PushButtonNAsync(ctx, Button.B, 1000, 3);
            }


            for (int i = days-1; i >= 0 && !ctx.IsCancellationRequested; i--)
            {
                // ワット回収
                await c.PushButtonAsync(ctx, Button.A, 1000);
                await c.PushButtonAsync(ctx, Button.B, 2000);
                await c.PushButtonAsync(ctx, Button.A, 1000);
                // 募集開始
                await c.PushButtonAsync(ctx, Button.A, 3000);

                await Common.Forward1Day(ctx, c, false, (countDate) =>
                {
                    var counted = countDate(clock, out DateTime next);

                    clock = next;
                    if (!ctx.IsCancellationRequested)
                    {
                        if (!counted)
                            i++;
                        Arguments[0].Value = clock.ToString("yyyy-MM-dd");
                    }
                });

                // レイド募集中止
                await c.PushButtonAsync(ctx, Button.B, 1000);
                await c.PushButtonAsync(ctx, Button.A, 4000);
            }

            // ポケモンを確認する
            await c.PushButtonAsync(ctx, Button.A, 1000);
            await c.PushButtonAsync(ctx, Button.B, 2000);
            await c.PushButtonAsync(ctx, Button.A, 1000);
        }
    }
}
