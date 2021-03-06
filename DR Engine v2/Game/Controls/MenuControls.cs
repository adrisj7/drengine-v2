﻿using GameEngine.Game;
using GameEngine.Game.Input;
using Microsoft.Xna.Framework.Input;

namespace DREngine.Game.Controls
{
    public class MenuControls : GameEngine.Game.Input.Controls
    {
        public InputActionButton Cancel;

        public InputActionButton MouseSelect;
        public InputActionButton MoveDown;
        public InputActionButton MoveLeft;
        public InputActionButton MoveRight;
        public InputActionButton MoveUp;

        public InputActionButton Select;

        public MenuControls(GamePlus game) : base(game)
        {
            MoveUp = new InputActionButton(this, Keys.Up, Buttons.DPadUp, Buttons.LeftThumbstickUp);
            MoveDown = new InputActionButton(this, Keys.Down, Buttons.DPadDown, Buttons.LeftThumbstickDown);
            MoveLeft = new InputActionButton(this, Keys.Left, Buttons.DPadLeft, Buttons.LeftThumbstickLeft);
            MoveRight = new InputActionButton(this, Keys.Right, Buttons.DPadRight, Buttons.LeftThumbstickRight);

            MouseSelect = new InputActionButton(this, MouseButton.Left);

            Select = new InputActionButton(this, Keys.Space, Keys.Enter, Keys.Z, Buttons.A);
            Cancel = new InputActionButton(this, Keys.Escape, Keys.Back, Keys.X, Buttons.B);
        }
    }
}