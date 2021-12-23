using UnityEngine;
using UnityEngine.Audio;
using System;


namespace MaksK_Race
{
    public sealed class AudioHelper : MonoBehaviour
    {
        public Sound[] sounds;

        private void Awake()
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
                if (s.source.playOnAwake) s.source.Play();
                s.source.spatialBlend = 1.0f;                
                s.source.outputAudioMixerGroup = s.output;                
            }
        }

        public void Play (string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
                return;
            s.source.Play();
        }

        public void Pitch (string name, float volume)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
                return;
            s.source.pitch = volume;
        }
    }
}
