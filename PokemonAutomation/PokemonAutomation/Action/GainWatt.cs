using SwitchController;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    public class GainWatt : IAction
    {
        public GainWatt()
        {
            Arguments = new ActionArgument[]
            {
                new ActionArgument("現在の日付")
                {
                    Value = DateTime.Now.ToString("yyyy-MM-dd")
                },
                new ActionArgument("短縮版")
                {
                    Value = false.ToString()
                },
            };
        }
        public ActionArgument[] Arguments { get; }

        public string Name => "ワット稼ぎ";

        public string Description => @"ワットを稼ぐ

初期条件は以下の通り
1.ねがいのかたまりを入れた巣の前にいること
";


        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            if (!DateTime.TryParseExact(Arguments[0].Value, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime clock))
            {
                throw new Exception(string.Format("invalid format: {0} must be yyyy-MM-dd", Arguments[0].Name));
            }
            if (!bool.TryParse(Arguments[1].Value, out bool shorten))
            {
                throw new Exception(string.Format("invalid format: {0} must be boolean", Arguments[1].Name));
            }

            Func<Common.CountDate, bool> onCountDate = (countDate) =>
            {
                var counted = countDate(clock, out DateTime next);

                clock = next;
                if (!ctx.IsCancellationRequested)
                {
                    Arguments[0].Value = clock.ToString("yyyy-MM-dd");
                }
                return !counted;
            };

            await c.PushButtonNAsync(ctx, Button.B, 1000, 4);

            while (!ctx.IsCancellationRequested)
            {
                // ワット回収
                await c.PushButtonAsync(ctx, Button.A, 1000);
                await c.PushButtonAsync(ctx, Button.B, 2000);
                await c.PushButtonAsync(ctx, Button.A, 1000);

                if (shorten)
                {
                    await c.PushButtonAsync(ctx, Button.B, 2500);

                    await Common.Forward1Day(ctx, c, false, onCountDate);
                }
                else
                {
                    // 募集開始
                    await c.PushButtonAsync(ctx, Button.A, 3000);

                    await Common.Forward1Day(ctx, c, false, onCountDate);

                    // レイド募集中止
                    await c.PushButtonAsync(ctx, Button.B, 1000);
                    await c.PushButtonAsync(ctx, Button.A, 4000);
                }
            }
        }
    }
}
