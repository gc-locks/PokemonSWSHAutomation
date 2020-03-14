using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwitchController
{
    public static class ControllerUtil
    {
        static int ButtonDelay = 500;

        public static bool Delay(CancellationToken ctx, int time)
        {
            if (ctx.IsCancellationRequested)
                return true;
            Task.Delay(time);
            return ctx.IsCancellationRequested;
        }
        public static void PushButton(this IController c, CancellationToken ctx, Button button, int delay)
        {
            if (ctx.IsCancellationRequested)
                return;

            c.InputButton(button, ButtonState.PRESS);
            if (Delay(ctx, ButtonDelay))
                return;

            c.InputButton(button, ButtonState.RELEASE);
            if (Delay(ctx, delay))
                return;
        }
        public static void PushButtonN(this IController c, CancellationToken ctx, Button button, int delay, int count)
        {
            if (ctx.IsCancellationRequested)
                return;

            for (int i = 0; i < count; i++)
            {
                c.PushButton(ctx, button, delay);
            }
        }
    }
}
