using System;
using Gtk;

namespace DREngine.Editor
{
    public class Text : Label
    {
        public const int Normal = 0,
            Warning = 1,
            Error = 2;

        private string _text;

        public Text(string text) : base(text)
        {
            _text = text;
            Wrap = true;
        }

        public void SetMode(int mode)
        {
            switch (mode)
            {
                case Normal:
                    ResetColor();
                    break;
                case Warning:
                    SetStyle("#FFDD11");
                    break;
                case Error:
                    SetStyle("#FF2210", "bold", "ultrabold");
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Set text mode to {mode} which is out of range!");
            }
        }

        private void SetStyle(string color, string style = "normal", string weight = "normal")
        {
            UseMarkup = true;
            Markup = $"<span foreground=\"{color}\" font_style=\"{style}\" font_weight=\"{weight}\">{Text}</span>";
            //output.Buffer.InsertWithTags(ref start, message, errTag);
        }

        private void ResetColor()
        {
            UseMarkup = false;
            //this.Text = _text;
        }
    }
}