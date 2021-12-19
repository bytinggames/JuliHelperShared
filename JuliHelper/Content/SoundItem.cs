﻿using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelperShared
{
    public abstract class SoundItemAbstract : IDisposable
    {
        private float volume = 0.5f;
        private float pitch = 0f;
        private float pan = 0f;

        public float Volume { get => volume; set => volume = Math.Min(Math.Max(value, 0f), 1f); }
        public float Pitch { get => pitch; set => pitch = Math.Min(Math.Max(value, -1f), 1f); }
        public float Pan { get => pan; set => pan = Math.Min(Math.Max(value, -1f), 1f); }

        public abstract void Dispose();

        public abstract bool Play();
        public abstract bool Play(float relativeVolume, float relativePitch, float relativePan);

        public void ResetVolumePitchPan()
        {
            volume = 0.5f;
            pitch = 0f;
            pan = 0f;
        }
    }

    public static class SoundMaster
    {
        public static bool Muted { get; set; }
    }

    public class SoundItem : SoundItemAbstract
    {
        public static bool Muted;

        private SoundEffect soundEffect;

        public SoundEffect SoundEffect
        {
            get => soundEffect;
            set
            {
                // TODO: instead of Dispose(), make sure this getter is called the right way
                soundEffect = value;
            }
        }

        public SoundItem(SoundEffect soundEffect)
        {
            SoundEffect = soundEffect;
        }

        public override void Dispose()
        {
            SoundEffect?.Dispose();
        }

        public override bool Play()
        {
            if (soundEffect == null)
                return false;

            if (SoundMaster.Muted)
                return true;

            return soundEffect.Play(Volume, Pitch, Pan);
        }
        public override bool Play(float relativeVolume, float relativePitch, float relativePan)
        {
            if (soundEffect == null)
                return false;

            if (SoundMaster.Muted)
                return true;

            return soundEffect.Play(
                Math.Min(Math.Max(Volume + relativeVolume, 0f), 1f)
                , Math.Min(Math.Max(Pitch + relativePitch, -1f), 1f)
                , Math.Min(Math.Max(Pan + relativePan, -1f), 1f));
        }
    }

    public class SoundItemCollection : SoundItemAbstract
    {
        static Random rand = new Random();

        private SoundEffect[] soundEffects;

        public SoundEffect[] SoundEffects
        {
            get => soundEffects;
            set
            {
                if (soundEffects != null)
                {
                    for (int i = 0; i < soundEffects.Length; i++)
                    {
                        soundEffects[i]?.Dispose();
                    }
                }
                soundEffects = value;
            }
        }

        public SoundItemCollection(SoundEffect[] soundEffects)
        {
            SoundEffects = soundEffects;
        }

        public override void Dispose()
        {
            for (int i = 0; i < soundEffects.Length; i++)
            {
                soundEffects[i]?.Dispose();
            }
        }

        private SoundEffect GetRandomSoundEffect() => soundEffects[rand.Next(soundEffects.Length)];

        public override bool Play()
        {
            if (SoundMaster.Muted)
                return true;

            return GetRandomSoundEffect().Play(Volume, Pitch, Pan);
        }
        public override bool Play(float relativeVolume, float relativePitch, float relativePan)
        {
            if (SoundMaster.Muted)
                return true;

            return GetRandomSoundEffect().Play(
                Math.Min(Math.Max(Volume + relativeVolume, 0f), 1f)
                , Math.Min(Math.Max(Pitch + relativePitch, -1f), 1f)
                , Math.Min(Math.Max(Pan + relativePan, -1f), 1f));
        }
    }
}
