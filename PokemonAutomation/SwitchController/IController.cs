using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchController
{
    public interface IController
    {
        void InputButton(Button button, ButtonState state);
        void InputHat(HatState hatState);
        void InputStick(Stick hatSwitch, StickState hatStateX, StickState hatStateY);
    }
}
