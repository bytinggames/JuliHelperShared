using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelperShared
{
    public class SoundItem : IDisposable
    {
        private SoundEffect soundEffect;
        private float volume = 0.5f;
        private float pitch = 0f;
        private float pan = 0f;

        public SoundEffect SoundEffect
        {
            get => soundEffect;
            set
            {
                soundEffect?.Dispose();
                soundEffect = value;
            }
        }

        public float Volume { get => volume; set => volume = Math.Clamp(value, 0f, 1f); }
        public float Pitch { get => pitch; set => pitch = Math.Clamp(value, -1f, 1f); }
        public float Pan { get => pan; set => pan = Math.Clamp(value, -1f, 1f); }

        public SoundItem(SoundEffect soundEffect)
        {
            SoundEffect = soundEffect;
        }

        public void Dispose()
        {
            SoundEffect.Dispose();
        }

        public bool Play()
        {
            return soundEffect.Play(Volume, Pitch, Pan);
        }
        public bool Play(float relativeVolume, float relativePitch, float relativePan)
        {
            return soundEffect.Play(
                Math.Clamp(Volume + relativeVolume, 0f, 1f)
                , Math.Clamp(Pitch + relativePitch, -1f, 1f)
                , Math.Clamp(Pan + relativePan, -1f, 1f));
        }
    }
}
