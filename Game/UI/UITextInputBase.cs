﻿using System;
using DREngine.Game.Input;
using Microsoft.Xna.Framework.Input;

namespace DREngine.Game.UI
{
    public abstract class UITextInputBase : UIMenuButtonBase
    {

        public abstract string Text { get; set; }

        public Action<string> Submitted;

        public bool Selected { get; private set; }

        public UITextInputBase(GamePlus game, UIComponent parent = null) : base(game, parent)
        {
            RawInput.OnKeysPressed += OnInput;
            Selected = false;
        }

        ~UITextInputBase()
        {
            // ReSharper disable once DelegateSubtraction
            RawInput.OnKeysPressed -= OnInput;
        }

        public void Select()
        {
            Selected = true;
        }

        public void Deselect()
        {
            Selected = false;
        }

        private void OnInput(Keys[] obj)
        {
            if (!Selected) return;
            // TODO: Caps lock?
            bool shift = RawInput.KeyPressing(Keys.LeftShift) || RawInput.KeyPressing(Keys.RightShift);
            bool ctrl = RawInput.KeyPressing(Keys.LeftControl) || RawInput.KeyPressing(Keys.RightControl);
            // We got keys boys
            foreach (Keys key in obj)
            {
                // Check for special keys
                switch (key)
                {
                    case Keys.LeftShift:
                    case Keys.RightShift:
                    case Keys.RightControl:
                    case Keys.LeftControl:
                        continue;
                    case Keys.Enter:
                        Submitted?.Invoke(Text);
                        OnSubmit();
                        break;
                    case Keys.Back:
                        OnBackspace(ctrl);
                        break;
                    case Keys.Left:
                        OnLeft(ctrl, shift);
                        break;
                    case Keys.Right:
                        OnRight(ctrl, shift);
                        break;
                    case Keys.Tab:
                        OnTab();
                        break;
                    case Keys.Home:
                        OnHome(shift);
                        break;
                    case Keys.End:
                        OnEnd(shift);
                        break;
                    default:
                        char c = key.ToChar(shift);
                        if (ctrl)
                        {
                            OnControlInput(c);
                        }
                        else
                        {
                            OnCharacterInput(c);
                        }
                        break;
                }
            }
        }


        protected abstract void OnCharacterInput(char c);
        protected abstract void OnControlInput(char c);
        protected abstract void OnSubmit();
        protected abstract void OnBackspace(bool ctrl);
        protected abstract void OnLeft(bool ctrl, bool shift);
        protected abstract void OnRight(bool ctrl, bool shift);
        protected abstract void OnTab();
        protected abstract void OnHome(bool shift);
        protected abstract void OnEnd(bool shift);

    }
}
