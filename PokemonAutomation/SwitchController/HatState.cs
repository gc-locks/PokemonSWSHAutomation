using System.Collections.Generic;

namespace SwitchController
{
    public enum NativeHatState : byte
    {
        Top = 0x00,
        TopRight = 0x01,
        Right = 0x02,
        BottomRight = 0x03,
        Bottom = 0x04,
        BottomLeft = 0x05,
        Left = 0x06,
        TopLeft = 0x07,
        Center = 0x08
    }
    public enum HatState : byte
    {
        Center = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
    }

    public static class HatStateUtil
    {
        private static readonly Dictionary<HatState, NativeHatState> toNative = new Dictionary<HatState, NativeHatState>()
        {
            { HatState.Up, NativeHatState.Top },
            { HatState.Up|HatState.Right, NativeHatState.TopRight },
            { HatState.Right, NativeHatState.Right },
            { HatState.Down|HatState.Right, NativeHatState.BottomRight },
            { HatState.Down, NativeHatState.Bottom },
            { HatState.Down|HatState.Left, NativeHatState.BottomLeft },
            { HatState.Left, NativeHatState.Left },
            { HatState.Up|HatState.Left, NativeHatState.TopLeft },
            { HatState.Center, NativeHatState.Center },
        };

        public static NativeHatState ToNativeHatState(HatState s)
        {
            if (toNative.TryGetValue(s, out NativeHatState ns))
            {
                return ns;
            }
            return NativeHatState.Center;
        }
    }
}
