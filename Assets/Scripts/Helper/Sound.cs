using UnityEngine;
using UnityEngine.Audio;


namespace Old_Code
{
    [System.Serializable]
    public sealed class Sound
    {
        public string name;

        public AudioClip clip;
        public AudioMixerGroup output;

        [HideInInspector]
        public AudioSource source;

        [Range(0.0f, 1.0f)]
        public float volume;
        [Range(0.0f, 2.0f)]
        public float pitch;

        public bool loop;
        public bool playOnAwake;
    }
}
