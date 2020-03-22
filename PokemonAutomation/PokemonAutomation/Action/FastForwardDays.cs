using SwitchController;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    class FastForwardDays : IAction
    {
        public FastForwardDays()
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
            };
        }

        public string Name => "N日進める(高速)";

        public string Description => @"
レートバトルバグを用いてN日時計をすすめる

条件
1. レートバトルをした後であること
2. Aボタンで時刻変更ダイアログが出せる状態であること
";

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

            await c.PushButtonAsync(ctx, Button.A, 500);
            await c.PushHatTAsync(ctx, HatState.Right, 1500, 100);
            await c.PushButtonAsync(ctx, Button.A, 500);

            for (int i = days-1; i >= 0 && !ctx.IsCancellationRequested; i--)
            {
                await c.PushButtonAsync(ctx, Button.A, 500);
                await c.PushHatNAsync(ctx, HatState.Left, 100, 3);
                await ControllerUtil.DelayAsync(ctx, 200);
                await c.PushHatAsync(ctx, HatState.Up, 100);
                await c.PushHatTAsync(ctx, HatState.Right, 1000, 0);
                await c.PushButtonAsync(ctx, Button.A, 500);

                if (!ctx.IsCancellationRequested)
                {
                    var nextDay = new DateTime(clock.Year, clock.Month, clock.Day).AddDays(1);
                    if (nextDay.Month != clock.Month)
                    {
                        nextDay = nextDay.AddMonths(-1);
                        i++;
                    }
                    clock = nextDay;

                    Arguments[0].Value = clock.ToString("yyyy-MM-dd");
                    Arguments[1].Value = i.ToString();
                }
            }
        }
    }
}
