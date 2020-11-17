using SwitchController;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAutomation.Action
{
    internal static class Common
    {
        public delegate bool CountDate(DateTime current, out DateTime next);
        public static async Task Forward1Day(CancellationToken ctx, IController c, bool twice, Func<CountDate, bool> onChangeDate = null)
        {
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

            bool twice2 = false;
            if (onChangeDate != null && !ctx.IsCancellationRequested)
                twice2 = onChangeDate(CountDateImpl);

            if (twice || twice2)
            {
                // 月末の可能性を考慮して二日進める
                await c.PushButtonAsync(ctx, Button.A, 500);
                await c.PushHatNAsync(ctx, HatState.Left, 100, 3);
                await ControllerUtil.DelayAsync(ctx, 200);
                await c.PushHatAsync(ctx, HatState.Up, 100);
                await c.PushHatTAsync(ctx, HatState.Right, 1000, 0);
                await c.PushButtonAsync(ctx, Button.A, 500);

                if (onChangeDate != null && !ctx.IsCancellationRequested)
                    onChangeDate(CountDateImpl);
            }

            // ホーム画面 > ゲーム画面
            await c.PushButtonAsync(ctx, Button.Home, 1000);
            await c.PushButtonAsync(ctx, Button.A, 500);
        }

        public static async Task ForwardDays(CancellationToken ctx, IController c, int days, Func<CountDate, bool> onChangeDate = null)
        {
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

            bool theLastDay = false;
            if (onChangeDate != null && !ctx.IsCancellationRequested)
                theLastDay = onChangeDate(CountDateImpl);
            // 月末の可能性を考慮して二日進める
            if (theLastDay && days < 2)
                days = 2;

            for (int i = 0; i < days-1; i++)
            {
                await c.PushButtonAsync(ctx, Button.A, 500);
                await c.PushHatNAsync(ctx, HatState.Left, 100, 3);
                await ControllerUtil.DelayAsync(ctx, 200);
                await c.PushHatAsync(ctx, HatState.Up, 100);
                await c.PushHatTAsync(ctx, HatState.Right, 1000, 0);
                await c.PushButtonAsync(ctx, Button.A, 500);

                if (onChangeDate != null && !ctx.IsCancellationRequested)
                    onChangeDate(CountDateImpl);
            }

            // ホーム画面 > ゲーム画面
            await c.PushButtonAsync(ctx, Button.Home, 1000);
            await c.PushButtonAsync(ctx, Button.A, 500);
        }

        private static bool CountDateImpl(DateTime current, out DateTime next)
        {
            next = new DateTime(current.Year, current.Month, current.Day).AddDays(1);
            if (next.Month == current.Month)
                return true;

            next = next.AddMonths(-1);
            return false;
        }
    }
}
