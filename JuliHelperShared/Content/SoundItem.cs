using LD48;
using Microsoft.Xna.Framework.Audio;
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

        public float Volume { get => volume; set => volume = Math.Clamp(value, 0f, 1f); }
        public float Pitch { get => pitch; set => pitch = Math.Clamp(value, -1f, 1f); }
        public float Pan { get => pan; set => pan = Math.Clamp(value, -1f, 1f); }

        public abstract void Dispose();

        public abstract bool Play();
        public abstract bool Play(float relativeVolume, float relativePitch, float relativePan);
    }

    public class SoundItem : SoundItemAbstract
    {
        private SoundEffect soundEffect;

        public SoundEffect SoundEffect
        {
            get => soundEffect;
            set
            {
                soundEffect?.Dispose();
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
            return soundEffect.Play(Volume, Pitch, Pan);
        }
        public override bool Play(float relativeVolume, float relativePitch, float relativePan)
        {
            if (soundEffect == null)
                return false;
            return soundEffect.Play(
                Math.Clamp(Volume + relativeVolume, 0f, 1f)
                , Math.Clamp(Pitch + relativePitch, -1f, 1f)
                , Math.Clamp(Pan + relativePan, -1f, 1f));
        }
    }

    public class SoundItemCollection : SoundItemAbstract
    {
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

        private SoundEffect GetRandomSoundEffect() => soundEffects[G.Rand.Next(soundEffects.Length)];

        public override bool Play()
        {
            return GetRandomSoundEffect().Play(Volume, Pitch, Pan);
        }
        public override bool Play(float relativeVolume, float relativePitch, float relativePan)
        {
            return GetRandomSoundEffect().Play(
                Math.Clamp(Volume + relativeVolume, 0f, 1f)
                , Math.Clamp(Pitch + relativePitch, -1f, 1f)
                , Math.Clamp(Pan + relativePan, -1f, 1f));
        }
    }
}
