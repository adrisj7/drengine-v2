using System;
using DREngine.Editor.SubWindows.FieldWidgets;
using GameEngine;
using GameEngine.Game.Audio;
using Gtk;

namespace DREngine.Editor.SubWindows.Resources
{
    public class AudioClipResourceWindow : ResourceWindow<AudioClip>
    {
        private DREditor _editor;

        private Button _playButton;
        private FieldBox _fields;

        private Image _playImage;
        private Image _stopImage;

        public AudioClipResourceWindow(DREditor editor, ProjectPath resPath) : base(editor, resPath)
        {
            _editor = editor;
            _playImage = new Image(_editor.Icons.Play);
            _stopImage = new Image(_editor.Icons.Stop);
        }

        protected override void OnInitialize(Box container)
        {
            _playButton = new Button();
            _playButton.Image = _playImage;

            _fields = new ExtraDataFieldBox(_editor, typeof(AudioClip), true);

            container.PackStart(_playButton, false, false,16);
            container.PackStart(_fields, false, true, 16);
            
            _playButton.Show();
            _fields.Show();

            _playButton.Pressed += (sender, args) =>
            {
                // Toggle
                bool playing = _editor.GlobalAudioSource.Playing;
                if (playing)
                {
                    _editor.GlobalAudioSource.Stop();
                    _playButton.Image = _playImage;
                }
                else
                {
                    _editor.GlobalAudioSource.Play(CurrentResource, () =>
                    {
                        _playButton.Image = _playImage;
                    });
                    _playButton.Image = _stopImage;
                }
            };

            _fields.Modified += MarkDirty;
        }

        protected override void OnLoadError(bool fileExists, Exception exception)
        {
            
        }

        protected override void OnClose()
        {
            _editor.GlobalAudioSource.Stop();
        }

        protected override void OnOpen(AudioClip resource, Box container)
        {
            _editor.GlobalAudioSource.Stop();
            _fields.LoadTarget(resource);
        }
    }
}