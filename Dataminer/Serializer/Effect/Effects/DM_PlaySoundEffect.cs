using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_PlaySoundEffect : DM_Effect
    {
        public List<GlobalAudioManager.Sounds> Sounds = new List<GlobalAudioManager.Sounds>();
        public bool Follow;
        public float MinPitch;
        public float MaxPitch;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            var template = holder as DM_PlaySoundEffect;
            var comp = effect as PlaySoundEffect;

            template.Follow = comp.Follow;
            template.MaxPitch = comp.MaxPitch;
            template.MinPitch = comp.MinPitch;
            template.Sounds = comp.Sounds.ToList();
        }
    }
}
