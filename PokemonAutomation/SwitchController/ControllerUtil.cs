using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwitchController
{
    public static class ControllerUtil
    {
        static int ButtonDelay = 50;

        public static async Task DelayAsync(CancellationToken ctx, int time)
        {
            if (ctx.IsCancellationRequested)
                return;
            await Task.Delay(time);
            return;
        }

        public static async Task ClearAsync(this IController c)
        {
            c.InputHat(HatState.Center);
            await Task.Delay(50);
            foreach (var e in Enum.GetValues(typeof(Stick)))
            {
                c.InputStick((Stick)e, StickState.CENTER, StickState.CENTER);
                await Task.Delay(50);
            }
            foreach (var e in Enum.GetValues(typeof(Button)))
            {
                c.InputButton((Button)e, ButtonState.RELEASE);
                await Task.Delay(50);
            }
        }
        public static async Task PushButtonAsync(this IController c, CancellationToken ctx, Button button, int delay)
        {
            if (ctx.IsCancellationRequested)
                return;

            Debug.WriteLine("Push: " + button.ToString());

            c.InputButton(button, ButtonState.PRESS);

            await DelayAsync(ctx, ButtonDelay);
            if (ctx.IsCancellationRequested)
                return;

            c.InputButton(button, ButtonState.RELEASE);
            await DelayAsync(ctx, delay);
            if (ctx.IsCancellationRequested)
                return;
        }
        public static async Task PushButtonNAsync(this IController c, CancellationToken ctx, Button button, int delay, int count)
        {
            if (ctx.IsCancellationRequested)
                return;

            for (int i = 0; i < count; i++)
            {
                await c.PushButtonAsync(ctx, button, delay);
            }
        }
        public static async Task PushHatAsync(this IController c, CancellationToken ctx, HatState s, int delay)
        {
            await c.PushHatTAsync(ctx, s, ButtonDelay, delay);
        }

        public static async Task PushHatTAsync(this IController c, CancellationToken ctx, HatState s, int time, int delay)
        {
            if (ctx.IsCancellationRequested)
                return;

            c.InputHat(s);
            await DelayAsync(ctx, time);
            if (ctx.IsCancellationRequested)
                return;

            c.InputHat(HatState.Center);
            await DelayAsync(ctx, delay);
            if (ctx.IsCancellationRequested)
                return;
        }
        public static async Task PushHatNAsync(this IController c, CancellationToken ctx, HatState s, int delay, int count)
        {
            if (ctx.IsCancellationRequested)
                return;

            for (int i = 0; i < count; i++)
            {
                await c.PushHatAsync(ctx, s, delay);
            }
        }
        public static void TiltStick(this IController c, CancellationToken ctx, Stick stick, float x, float y)
        {
            if (x > 1.0f || x < -1.0f || y > 1.0f || y < -1.0f)
            {
                throw new Exception();
            }

            if (ctx.IsCancellationRequested)
                return;

            Func<float, StickState> toByte = (float w) => (byte)(w * (StickState.MAX - StickState.CENTER)) + StickState.CENTER;
            c.InputStick(stick, toByte(x), toByte(y));
        }
    }
}
