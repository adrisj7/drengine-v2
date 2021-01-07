using System;
using DREngine.Editor.Components;
using DREngine.Editor.SubWindows.FieldWidgets;
using DREngine.ResourceLoading;
using GameEngine.Game.Resources;
using Gtk;

namespace DREngine.Editor.SubWindows.Resources
{
    public class SpriteResourceWindow : ResourceWindow<Sprite>
    {
        private readonly DREditor _editor;

        private FieldBox _fields;

        private Image _image;
        private Text _label;

        public SpriteResourceWindow(DREditor editor, ProjectPath resPath) : base(editor, resPath)
        {
            _editor = editor;
        }

        protected override void OnInitialize(Box container)
        {
            var scroll = new ScrolledWindow();
            scroll.MinContentWidth = 300;
            scroll.MinContentHeight = 300;
            _image = new Image();
            scroll.Add(_image);
            _label = new Text("Nothing loaded");
            _fields = new ExtraDataFieldBox(_editor, typeof(Sprite), true);

            container.PackStart(scroll, true, true, 16);
            //container.PackStart(_image, true, true, 4);
            container.PackEnd(_label, false, false, 16);
            container.PackEnd(_fields, false, true, 16);
            scroll.Show();
            _image.Show();
            _label.Show();
            _fields.Show();

            _fields.Modified += MarkDirty;
        }

        protected override void OnOpen(Sprite resource, Box container)
        {
            // Load sprite
            _image.File = resource.Path;

            // Scale
            var old = _image.Pixbuf;
            _image.Pixbuf = _editor.Icons.ScaleToRegularSize(_image.Pixbuf, 300);
            old.Dispose();

            _label.Text = $"{resource.Width} x {resource.Height} Sprite";
            _fields.LoadTarget(resource);
        }

        protected override void OnLoadError(bool fileExists, Exception exception)
        {
            // display error
            _image.Clear();
            _label.Text = $"ERROR: {exception}";
        }

        protected override void OnClose()
        {
            if (Dirty) CurrentResource?.Load(_editor.ResourceLoaderData);
        }
    }
}