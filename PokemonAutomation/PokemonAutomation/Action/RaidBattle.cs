using SwitchController;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    class RaidBattle : IAction
    {
        public RaidBattle()
        {
            Arguments = new ActionArgument[]
            {
                new ActionArgument("バトルする時間[s]")
                {
                    Value = "200"
                }
            };
        }

        public string Name => "レイドバトル";

        public string Description => @"レイドバトル→ポケモンを捕獲→ボックスに預ける→願いの塊を投げ入れるを繰り返すスケッチ
from https://github.com/interimadd/NintendoSwitchControll

ボックスに空きがある限り、レイドバトルを続ける

初期条件は以下の通り
1. 願いの塊を投げ入れた巣穴の前にいること
2. 手持ちが1体のみのこと
3. Aボタン連打でレイドバトルで勝てるようにすること
4. 願いの塊を大量に持っていること";

        public ActionArgument[] Arguments { get; }

        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            if (!int.TryParse(Arguments[0].Value, out int duration))
            {
                throw new Exception(string.Format("invalid format: {0} must be integer", Arguments[1].Name));
            }

            await c.PushButtonNAsync(ctx, Button.A, 500, 10);

            while (!ctx.IsCancellationRequested)
            {
                // レイドバトル後もしばらく続くAボタン連打の後の画面から、
                // 巣穴の前の最初のポジションに戻す
                await c.PushButtonNAsync(ctx, Button.B, 1000, 4);
                await ControllerUtil.DelayAsync(ctx, 1000);
                await c.PushButtonNAsync(ctx, Button.A, 1000, 2);
                await c.PushButtonNAsync(ctx, Button.B, 1000, 3);

                // 巣穴の前からひとりレイドを始め、レイドポケモンを倒し、捕まえる
                await c.PushButtonAsync(ctx, Button.A, 1500);
                await c.PushHatAsync(ctx, HatState.Down, 500);
                // レイドバトル中はA連打
                var endAt = DateTime.Now.AddSeconds(duration);
                while(!ctx.IsCancellationRequested && DateTime.Now < endAt)
                {
                    await c.PushButtonAsync(ctx, Button.A, 500);
                }
            }
        }
    }
}
