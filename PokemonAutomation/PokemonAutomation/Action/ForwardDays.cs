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

        public string Description => @"
巣穴を用いてN日時計をすすめる";

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
                // ホーム画面 > 設定
                await c.PushButtonAsync(ctx, Button.Home, 1000);
                await c.PushHatAsync(ctx, HatState.Down, 100);
                await c.PushHatNAsync(ctx, HatState.Right, 100, 4);
                await ControllerUtil.DelayAsync(ctx, 200);
                await c.PushButtonAsync(ctx, Button.A, 1000);
                // 設定 > 本体 > 日付と時刻
                await c.PushHatTAsync(ctx, HatState.Down, 2000, 0);
                await c.PushHatAsync(ctx, HatState.Right, 100);
                await c.PushHatNAsync(ctx, HatState.Down, 100, 4);
                await ControllerUtil.DelayAsync(ctx, 200);
                await c.PushButtonAsync(ctx, Button.A, 700);
                // 日付と時刻 > 現在の日付と時刻
                await c.PushHatNAsync(ctx, HatState.Down, 100, 2);
                await ControllerUtil.DelayAsync(ctx, 200);
                await c.PushButtonAsync(ctx, Button.A, 500);
                await c.PushHatNAsync(ctx, HatState.Right, 100, 2);
                await ControllerUtil.DelayAsync(ctx, 200);
                await c.PushHatAsync(ctx, HatState.Up, 100);
                await c.PushHatTAsync(ctx, HatState.Right, 1000, 0);
                await c.PushButtonAsync(ctx, Button.A, 500);

                var nextDay = new DateTime(clock.Year, clock.Month, clock.Day).AddDays(1);
                if (nextDay.Month != clock.Month)
                {
                    nextDay = nextDay.AddMonths(-1);
                    i++;
                }
                clock = nextDay;
                if (!ctx.IsCancellationRequested)
                    Arguments[0].Value = clock.ToString("yyyy-MM-dd");

                // ホーム画面 > ゲーム画面
                await c.PushButtonAsync(ctx, Button.Home, 1000);
                await c.PushButtonAsync(ctx, Button.A, 500);
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
