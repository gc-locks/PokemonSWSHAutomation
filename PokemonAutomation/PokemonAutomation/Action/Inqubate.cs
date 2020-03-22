using SwitchController;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    public class Inqubate : IAction
    {
        readonly ActionArgument[] arguments;
        public Inqubate()
        {
            arguments = new ActionArgument[]
            {
                new ActionArgument("孵化サイクル")
            };
        }

        public string Name => "孵化";

        public string Description => @"育て屋から卵を回収→孵化→ボックスに預けるを繰り返す from https://github.com/interimadd/NintendoSwitchControll
ボックスに空きがある限り、ポケモンを孵化し続ける

初期条件は以下の通り
1.ハシノマはらっぱにいること
2.自転車に乗っていること
3.手持ちが1体のみのこと
4.Xボタンを押したときに「タウンマップ」が左上、「ポケモン」がその右にあること
5.ボックスが空のこと
";

        public ActionArgument[] Arguments
        {
            get
            {
                return arguments;
            }
        }

        public async Task CallAsync(CancellationToken ctx, IController c)
        {
            if (!int.TryParse(this.arguments[0].Value, out int inqCycle))
            {
                throw new Exception();
            }

            await c.PushButtonNAsync(ctx, Button.B, 500, 5);
            await MoveIntoIntialPositionAsync(ctx, c, true);
            await RunAroundAsync(ctx, c, 20000);

            while (!ctx.IsCancellationRequested)
            {
                for (int j = 0; j < 6; j++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        await MoveIntoIntialPositionAsync(ctx, c, i == 0);
                        await GetEggAsync(ctx, c);
                        await RunAroundAsync(ctx, c, inqCycle * 3300);
                        await c.PushButtonNAsync(ctx, Button.B, 500, 35);
                    }

                    await SendToBoxAsync(ctx, c, j);
                }

                await MoveToNextBoxAsync(ctx, c);
            }
        }

        private async Task MoveIntoIntialPositionAsync(CancellationToken ctx, IController c, bool first)
        {
            await c.PushButtonAsync(ctx, Button.X, 1000);
            if (first)
                await c.PushHatTAsync(ctx, HatState.Up | HatState.Left, 1000, 100);
            await c.PushButtonAsync(ctx, Button.A, 2500);
            await c.PushButtonNAsync(ctx, Button.A, 1500, 2);
            await ControllerUtil.DelayAsync(ctx, 2000);
        }

        private async Task RunAroundAsync(CancellationToken ctx, IController c, int time)
        {
            // 野生ポケモンとのエンカウントを避けるため初期位置から少し移動する
            c.TiltStick(ctx, Stick.LSTICK, 1.0f, 0.0f);
            await ControllerUtil.DelayAsync(ctx, 600);

            c.TiltStick(ctx, Stick.LSTICK, 1.0f, 1.0f);
            c.TiltStick(ctx, Stick.RSTICK, -1.0f, -1.0f);
            await ControllerUtil.DelayAsync(ctx, time);
            c.TiltStick(ctx, Stick.LSTICK, 0.0f, 0.0f);
            c.TiltStick(ctx, Stick.RSTICK, 0.0f, 0.0f);
        }

        private async Task GetEggAsync(CancellationToken ctx, IController c)
        {
            // 初期位置(ハシノマはらっぱ)から育て屋さんのところまで移動
            await c.PushButtonAsync(ctx, Button.Plus, 1000);
            c.TiltStick(ctx, Stick.LSTICK, -0.1f, 1.0f);
            await ControllerUtil.DelayAsync(ctx, 2000);
            c.TiltStick(ctx, Stick.LSTICK, -0.7f, 0.1f);
            await ControllerUtil.DelayAsync(ctx, 1000);
            c.TiltStick(ctx, Stick.LSTICK, 0.0f, 0.0f);

            // 育て屋さんから卵をもらう
            await c.PushButtonNAsync(ctx, Button.A, 1500, 2);
            await c.PushButtonAsync(ctx, Button.A, 3000);
            await c.PushButtonNAsync(ctx, Button.B, 500, 8);
            c.TiltStick(ctx, Stick.LSTICK, 0.3f, -1.0f);
            await ControllerUtil.DelayAsync(ctx, 1500);
            c.TiltStick(ctx, Stick.LSTICK, 0.0f, 0.0f);
            await c.PushButtonAsync(ctx, Button.Plus, 1000);
        }

        private async Task SendToBoxAsync(CancellationToken ctx, IController c, int line)
        {
            await c.PushButtonAsync(ctx, Button.X, 1000);
            await c.PushHatAsync(ctx, HatState.Right, 500);
            await c.PushButtonAsync(ctx, Button.A, 2000);
            await c.PushButtonAsync(ctx, Button.R, 2000);

            // 手持ちの孵化したポケモンを範囲選択
            await c.PushHatAsync(ctx, HatState.Left, 500);
            await c.PushHatAsync(ctx, HatState.Down, 500);
            await c.PushButtonNAsync(ctx, Button.Y, 500, 2);

            await c.PushButtonAsync(ctx, Button.A, 500);
            await c.PushHatTAsync(ctx, HatState.Down, 2000, 0);
            await c.PushButtonAsync(ctx, Button.A, 500);

            // ボックスに移動させる
            await c.PushHatNAsync(ctx, HatState.Right, 500, line + 1);
            await c.PushHatAsync(ctx, HatState.Up, 500);
            await c.PushButtonAsync(ctx, Button.A, 500);

            // ボックスを閉じる
            await c.PushButtonNAsync(ctx, Button.B, 1500, 3);
        }

        private async Task MoveToNextBoxAsync(CancellationToken ctx, IController c)
        {
            await c.PushButtonAsync(ctx, Button.X, 1000);
            await c.PushButtonAsync(ctx, Button.A, 2000);
            await c.PushButtonAsync(ctx, Button.R, 2000);

            await c.PushHatAsync(ctx, HatState.Up, 500);
            await c.PushHatAsync(ctx, HatState.Right, 500);

            // ボックスを閉じる
            await c.PushButtonNAsync(ctx, Button.B, 1500, 3);
        }
    }
}
