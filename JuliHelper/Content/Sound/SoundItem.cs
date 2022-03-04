using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelperShared
{
    public class SoundItem : SoundItemAbstract
    {
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

            return soundEffect.Play(GetOutputVolume(), Pitch, Pan);
        }
        public override bool Play(float relativeVolume, float relativePitch, float relativePan)
        {
            if (soundEffect == null)
                return false;

            if (SoundMaster.Muted)
                return true;

            return soundEffect.Play(
                GetOutputVolume(Volume + relativeVolume)
                , Math.Min(Math.Max(Pitch + relativePitch, -1f), 1f)
                , Math.Min(Math.Max(Pan + relativePan, -1f), 1f));
        }

        public override bool Play(float volumeMultiplier)
        {
            if (soundEffect == null)
                return false;

            if (SoundMaster.Muted)
                return true;

            return soundEffect.Play(GetOutputVolume(Volume * volumeMultiplier), 0f, 0f);
        }
    }
}
