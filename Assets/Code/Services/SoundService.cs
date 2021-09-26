using System;
using Code.Data;
using Code.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Services
{
    public class SoundService : MonoBehaviour, IService
    {
#if UNITY_EDITOR
        [Multiline]
        public string UsageTip = "";
#endif
    
        public AudioMixer AudioMixer;

        [SerializeField] private Sound[] sounds;

        #region Class Logic
        public void Play(string track)
        {
            Sound sound = Array.Find(sounds, soundArray => soundArray.TrackName == track);
            if (sound == null) return;
            sound.Source.Play();
        }

        public void Play(string track, float speedEffect)
        {
            Sound sound = Array.Find(sounds, soundArray => soundArray.TrackName == track);
            if (sound == null) return;
            sound.FadeIn(speedEffect);
        }

        public void Stop(string track)
        {
            Sound sound = Array.Find(sounds, soundArray => soundArray.TrackName == track);
            if (sound == null) return;
            sound.Source.Stop();
        }
            
        public void Stop(string track, float speedEffect)
        {
            Sound sound = Array.Find(sounds, soundArray => soundArray.TrackName == track);
            if (sound == null) return;
            sound.FadeOut(speedEffect);
        }

        public void SetLooping(string track, bool status)
        {
            Sound sound = Array.Find(sounds, soundArray => soundArray.TrackName == track);
            if (sound == null) return;
            sound.Source.loop = status;
        }

        public bool IsPlaying(string track)
        {
            Sound sound = Array.Find(sounds, soundArray => soundArray.TrackName == track);
            if (sound == null) return false;
            
            return sound.Source.isPlaying;
        }
        #endregion

        #region MonoBehaviour API
        void Awake()
        {
            foreach(Sound sound in sounds) 
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.Clip;

                sound.Source.outputAudioMixerGroup = sound.MixerGroup;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.loop = sound.Loop;
            }
        }
        #endregion
    }
}
