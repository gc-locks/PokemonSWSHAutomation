using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchController
{
    public interface IController
    {
        void InputButton(Button button, ButtonState state);
        void InputHatSwitch(HatSwitch hatSwitch, HatState hatStateX, HatState hatStateY);
    }
}
