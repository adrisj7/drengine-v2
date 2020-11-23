﻿using DREngine.Game.Input;
using Microsoft.Xna.Framework.Input;

namespace DREngine.Game.Debugging
{
    public class DebugControls : Controls
    {
        public InputActionButton ConsoleOpen;
        public InputActionButton ConsoleClose;
        public InputActionButton ConsoleSubmit;

        public DebugControls(GamePlus _game) : base(_game)
        {
            ConsoleOpen = new InputActionButton(this, Keys.OemTilde);
            ConsoleClose = new InputActionButton(this, Keys.Escape);
            ConsoleSubmit = new InputActionButton(this, Keys.Enter);
        }
    }
}
