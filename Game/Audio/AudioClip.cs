﻿using System;
using ManagedBass;

namespace DREngine.Game.Audio
{
    /// <summary>
    /// Represents an audio clip that can store and stream audio in various ways.
    /// </summary>
    public class AudioClip
    {
        private AudioStorageBase _clip;

        private AudioClipType _type;

        public bool UsesSample { get; private set; }= true;

        // TODO: Add default volume and pitch scale
        public AudioClip(AudioOutput targetOutput, Path audioFile, AudioClipType type = AudioClipType.Cached)
        {
            _type = type;
            switch (type)
            {
                case AudioClipType.Cached:
                    _clip = new AudioStorageCached(targetOutput, audioFile);
                    UsesSample = true;
                    break;
                case AudioClipType.Streamed:
                    _clip = new AudioStorageStreamed(targetOutput, audioFile);
                    UsesSample = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public int GetNewSource()
        {
            if (UsesSample)
            {
                return Bass.SampleGetChannel(_clip.GetSample(), true);
            }

            return _clip.GetStream();
        }


    }

    public enum AudioClipType
    {
        Cached,
        Streamed
    }
}
