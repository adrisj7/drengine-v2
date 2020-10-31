﻿
using System.Text;

namespace DREngine.Game.UI
{
    /// <summary>
    /// The visual part of the debug console.
    /// </summary>
    public class UIDebugConsole : UiComponent
    {
        private StringBuilder _textLogBuffer = null;
        private StringBuilder _inputText = null;

        public UIDebugConsole(GamePlus game, UiComponent parent = null) : base(game, parent)
        {
            _textLogBuffer = new StringBuilder("Hello this is\n A command line Test");
        }

        protected override void Draw(UIScreen screen, Rect targetRect)
        {

        }
    }
}
